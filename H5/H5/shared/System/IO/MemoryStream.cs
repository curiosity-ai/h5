// ==++==
//
//   Copyright (c) Microsoft Corporation.  All rights reserved.
//
// ==--==
/*============================================================
**
** Class:  MemoryStream
**
** <OWNER>Microsoft</OWNER>
**
**
** Purpose: A Stream whose backing store is memory.  Great
** for temporary storage without creating a temp file.  Also
** lets users expose a byte[] as a stream.
**
**
===========================================================*/
/*
 * https://github.com/Microsoft/referencesource/blob/master/mscorlib/system/io/memorystream.cs
 */


using System;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;

namespace System.IO
{
    // A MemoryStream represents a Stream in memory (ie, it has no backing store).
    // This stream may reduce the need for temporary buffers and files in
    // an application.
    //
    // There are two ways to create a MemoryStream.  You can initialize one
    // from an unsigned byte array, or you can create an empty one.  Empty
    // memory streams are resizable, while ones created with a byte array provide
    // a stream "view" of the data.
    public class MemoryStream : Stream
    {
        private byte[] _buffer;    // Either allocated internally or externally.
        private int _origin;       // For user-provided arrays, start at this origin
        private int _position;     // read/write head.
        [ContractPublicPropertyName("Length")]
        private int _length;       // Number of bytes within the memory stream
        private int _capacity;     // length of usable portion of buffer for stream
        // Note that _capacity == _buffer.Length for non-user-provided byte[]'s

        private bool _expandable;  // User-provided buffers aren't expandable.
        private bool _writable;    // Can user write to this stream?
        private bool _exposable;   // Whether the array can be returned to the user.
        private bool _isOpen;      // Is this stream open or closed?

        private const int MemStreamMaxLength = int.MaxValue;

        public MemoryStream()
            : this(0)
        {
        }

        public MemoryStream(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity", "ArgumentOutOfRange_NegativeCapacity");
            }
            Contract.EndContractBlock();

            _buffer = new byte[capacity];
            _capacity = capacity;
            _expandable = true;
            _writable = true;
            _exposable = true;
            _origin = 0;      // Must be 0 for byte[]'s created by MemoryStream
            _isOpen = true;
        }

        public MemoryStream(byte[] buffer)
            : this(buffer, true)
        {
        }

        public MemoryStream(byte[] buffer, bool writable)
        {
            if (buffer == null) throw new ArgumentNullException("buffer", "ArgumentNull_Buffer");
            Contract.EndContractBlock();
            _buffer = buffer;
            _length = _capacity = buffer.Length;
            _writable = writable;
            _exposable = false;
            _origin = 0;
            _isOpen = true;
        }

        public MemoryStream(byte[] buffer, int index, int count)
            : this(buffer, index, count, true, false)
        {
        }

        public MemoryStream(byte[] buffer, int index, int count, bool writable)
            : this(buffer, index, count, writable, false)
        {
        }

        public MemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "ArgumentNull_Buffer");
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "ArgumentOutOfRange_NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_NeedNonNegNum");
            if (buffer.Length - index < count)
                throw new ArgumentException("Argument_InvalidOffLen");
            Contract.EndContractBlock();

            _buffer = buffer;
            _origin = _position = index;
            _length = _capacity = index + count;
            _writable = writable;
            _exposable = publiclyVisible;  // Can TryGetBuffer/GetBuffer return the array?
            _expandable = false;
            _isOpen = true;
        }

        public override bool CanRead
        {
            [Pure]
            get
            {
                return _isOpen;
            }
        }

        public override bool CanSeek
        {
            [Pure]
            get
            {
                return _isOpen;
            }
        }

        public override bool CanWrite
        {
            [Pure]
            get
            {
                return _writable;
            }
        }

        private void EnsureWriteable()
        {
            if (!CanWrite) __Error.WriteNotSupported();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _isOpen = false;
                    _writable = false;
                    _expandable = false;
                }
            }
            finally
            {
                // Call base.Close() to cleanup async IO resources
                base.Dispose(disposing);
            }
        }

        // returns a bool saying whether we allocated a new array.
        private bool EnsureCapacity(int value)
        {
            // Check for overflow
            if (value < 0)
                throw new IOException("IO.IO_StreamTooLong");
            if (value > _capacity)
            {
                int newCapacity = value;
                if (newCapacity < 256)
                    newCapacity = 256;
                // We are ok with this overflowing since the next statement will deal
                // with the cases where _capacity*2 overflows.
                if (newCapacity < _capacity * 2)
                    newCapacity = _capacity * 2;
                // We want to expand the array up to Array.MaxArrayLengthOneDimensional
                // And we want to give the user the value that they asked for
                if ((uint)(_capacity * 2) > 0x7FFFFFC7)
                    newCapacity = value > 0x7FFFFFC7 ? value : 0x7FFFFFC7;

                Capacity = newCapacity;
                return true;
            }
            return false;
        }

        public override void Flush()
        {
        }

        public virtual byte[] GetBuffer()
        {
            if (!_exposable)
                throw new Exception("UnauthorizedAccess_MemStreamBuffer");
            return _buffer;
        }

        public virtual bool TryGetBuffer(out ArraySegment<byte> buffer)
        {
            if (!_exposable)
            {
                buffer = default(ArraySegment<byte>);
                return false;
            }

            buffer = new ArraySegment<byte>(_buffer, offset: _origin, count: (_length - _origin));
            return true;
        }

        // -------------- PERF: Internal functions for fast direct access of MemoryStream buffer (cf. BinaryReader for usage) ---------------

        // PERF: Internal sibling of GetBuffer, always returns a buffer (cf. GetBuffer())
        internal byte[] InternalGetBuffer()
        {
            return _buffer;
        }

        // PERF: True cursor position, we don't need _origin for direct access
        internal int InternalGetPosition()
        {
            if (!_isOpen) __Error.StreamIsClosed();
            return _position;
        }

        // PERF: Takes out Int32 as fast as possible
        internal int InternalReadInt32()
        {
            if (!_isOpen)
                __Error.StreamIsClosed();

            int pos = (_position += 4); // use temp to avoid ----
            if (pos > _length)
            {
                _position = _length;
                __Error.EndOfFile();
            }
            return (int)(_buffer[pos - 4] | _buffer[pos - 3] << 8 | _buffer[pos - 2] << 16 | _buffer[pos - 1] << 24);
        }

        // PERF: Get actual length of bytes available for read; do sanity checks; shift position - i.e. everything except actual copying bytes
        internal int InternalEmulateRead(int count)
        {
            if (!_isOpen) __Error.StreamIsClosed();

            int n = _length - _position;
            if (n > count) n = count;
            if (n < 0) n = 0;

            Contract.Assert(_position + n >= 0, "_position + n >= 0");  // len is less than 2^31 -1.
            _position += n;
            return n;
        }

        // Gets & sets the capacity (number of bytes allocated) for this stream.
        // The capacity cannot be set to a value less than the current length
        // of the stream.
        //
        public virtual int Capacity
        {
            get
            {
                if (!_isOpen) __Error.StreamIsClosed();
                return _capacity - _origin;
            }
            set
            {
                // Only update the capacity if the MS is expandable and the value is different than the current capacity.
                // Special behavior if the MS isn't expandable: we don't throw if value is the same as the current capacity
                if (value < Length) throw new ArgumentOutOfRangeException("value", "ArgumentOutOfRange_SmallCapacity");
                Contract.Ensures(_capacity - _origin == value);
                Contract.EndContractBlock();

                if (!_isOpen) __Error.StreamIsClosed();
                if (!_expandable && (value != Capacity)) __Error.MemoryStreamNotExpandable();

                // MemoryStream has this invariant: _origin > 0 => !expandable (see ctors)
                if (_expandable && value != _capacity)
                {
                    if (value > 0)
                    {
                        byte[] newBuffer = new byte[value];
                        if (_length > 0) Array.Copy(_buffer, 0, newBuffer, 0, _length);
                        _buffer = newBuffer;
                    }
                    else
                    {
                        _buffer = null;
                    }
                    _capacity = value;
                }
            }
        }

        public override long Length
        {
            get
            {
                if (!_isOpen) __Error.StreamIsClosed();
                return _length - _origin;
            }
        }

        public override long Position
        {
            get
            {
                if (!_isOpen) __Error.StreamIsClosed();
                return _position - _origin;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "ArgumentOutOfRange_NeedNonNegNum");
                Contract.Ensures(Position == value);
                Contract.EndContractBlock();

                if (!_isOpen) __Error.StreamIsClosed();

                if (value > MemStreamMaxLength)
                    throw new ArgumentOutOfRangeException("value", "ArgumentOutOfRange_StreamLength");
                _position = _origin + (int)value;
            }
        }

        public override int Read([In, Out] byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "ArgumentNull_Buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "ArgumentOutOfRange_NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_NeedNonNegNum");
            if (buffer.Length - offset < count)
                throw new ArgumentException("Argument_InvalidOffLen");
            Contract.EndContractBlock();

            if (!_isOpen) __Error.StreamIsClosed();

            int n = _length - _position;
            if (n > count) n = count;
            if (n <= 0)
                return 0;

            Contract.Assert(_position + n >= 0, "_position + n >= 0");  // len is less than 2^31 -1.

            if (n <= 8)
            {
                int byteCount = n;
                while (--byteCount >= 0)
                    buffer[offset + byteCount] = _buffer[_position + byteCount];
            }
            else
                Array.Copy(_buffer, _position, buffer, offset, n);
            _position += n;

            return n;
        }

        public override int ReadByte()
        {
            if (!_isOpen) __Error.StreamIsClosed();

            if (_position >= _length) return -1;

            return _buffer[_position++];
        }

        public override long Seek(long offset, SeekOrigin loc)
        {
            if (!_isOpen) __Error.StreamIsClosed();

            if (offset > MemStreamMaxLength)
                throw new ArgumentOutOfRangeException("offset", "ArgumentOutOfRange_StreamLength");
            switch (loc)
            {
                case SeekOrigin.Begin:
                    {
                        int tempPosition = unchecked(_origin + (int)offset);
                        if (offset < 0 || tempPosition < _origin)
                            throw new IOException("IO.IO_SeekBeforeBegin");
                        _position = tempPosition;
                        break;
                    }
                case SeekOrigin.Current:
                    {
                        int tempPosition = unchecked(_position + (int)offset);
                        if (unchecked(_position + offset) < _origin || tempPosition < _origin)
                            throw new IOException("IO.IO_SeekBeforeBegin");
                        _position = tempPosition;
                        break;
                    }
                case SeekOrigin.End:
                    {
                        int tempPosition = unchecked(_length + (int)offset);
                        if (unchecked(_length + offset) < _origin || tempPosition < _origin)
                            throw new IOException("IO.IO_SeekBeforeBegin");
                        _position = tempPosition;
                        break;
                    }
                default:
                    throw new ArgumentException("Argument_InvalidSeekOrigin");
            }

            Contract.Assert(_position >= 0, "_position >= 0");
            return _position;
        }

        // Sets the length of the stream to a given value.  The new
        // value must be nonnegative and less than the space remaining in
        // the array, Int32.MaxValue - origin
        // Origin is 0 in all cases other than a MemoryStream created on
        // top of an existing array and a specific starting offset was passed
        // into the MemoryStream constructor.  The upper bounds prevents any
        // situations where a stream may be created on top of an array then
        // the stream is made longer than the maximum possible length of the
        // array (Int32.MaxValue).
        //
        public override void SetLength(long value)
        {
            if (value < 0 || value > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("value", "ArgumentOutOfRange_StreamLength");
            }
            Contract.Ensures(_length - _origin == value);
            Contract.EndContractBlock();
            EnsureWriteable();

            // Origin wasn't publicly exposed above.
            Contract.Assert(MemStreamMaxLength == int.MaxValue);  // Check parameter validation logic in this method if this fails.
            if (value > (int.MaxValue - _origin))
            {
                throw new ArgumentOutOfRangeException("value", "ArgumentOutOfRange_StreamLength");
            }

            int newLength = _origin + (int)value;
            bool allocatedNewArray = EnsureCapacity(newLength);
            if (!allocatedNewArray && newLength > _length)
                Array.Clear(_buffer, _length, newLength - _length);
            _length = newLength;
            if (_position > newLength) _position = newLength;

        }

        public virtual byte[] ToArray()
        {
            byte[] copy = new byte[_length - _origin];
            Array.Copy(_buffer, _origin, copy, 0, _length - _origin);
            return copy;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer", "ArgumentNull_Buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset", "ArgumentOutOfRange_NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "ArgumentOutOfRange_NeedNonNegNum");
            if (buffer.Length - offset < count)
                throw new ArgumentException("Argument_InvalidOffLen");
            Contract.EndContractBlock();

            if (!_isOpen) __Error.StreamIsClosed();
            EnsureWriteable();

            int i = _position + count;
            // Check for overflow
            if (i < 0)
                throw new IOException("IO.IO_StreamTooLong");

            if (i > _length)
            {
                bool mustZero = _position > _length;
                if (i > _capacity)
                {
                    bool allocatedNewArray = EnsureCapacity(i);
                    if (allocatedNewArray)
                        mustZero = false;
                }
                if (mustZero)
                    Array.Clear(_buffer, _length, i - _length);
                _length = i;
            }
            if ((count <= 8) && (buffer != _buffer))
            {
                int byteCount = count;
                while (--byteCount >= 0)
                    _buffer[_position + byteCount] = buffer[offset + byteCount];
            }
            else
                Array.Copy(buffer, offset, _buffer, _position, count);
            _position = i;

        }

        public override void WriteByte(byte value)
        {
            if (!_isOpen) __Error.StreamIsClosed();
            EnsureWriteable();

            if (_position >= _length)
            {
                int newLength = _position + 1;
                bool mustZero = _position > _length;
                if (newLength >= _capacity)
                {
                    bool allocatedNewArray = EnsureCapacity(newLength);
                    if (allocatedNewArray)
                        mustZero = false;
                }
                if (mustZero)
                    Array.Clear(_buffer, _length, _position - _length);
                _length = newLength;
            }
            _buffer[_position++] = value;

        }

        // Writes this MemoryStream to another stream.
        public virtual void WriteTo(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream", "ArgumentNull_Stream");
            Contract.EndContractBlock();

            if (!_isOpen) __Error.StreamIsClosed();
            stream.Write(_buffer, _origin, _length - _origin);
        }
    }
}
