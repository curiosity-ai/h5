using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [IgnoreCast]
        [Virtual]
        public abstract class CanvasImageSource : TypeAlias<IObject>
        {
            public static extern implicit operator dom.CanvasImageSource(dom.HTMLImageElement value);
            public static extern implicit operator dom.CanvasImageSource(dom.HTMLVideoElement value);
            public static extern implicit operator dom.CanvasImageSource(dom.HTMLCanvasElement value);
            // Add others like ImageBitmap, OffscreenCanvas if they exist or when they exist
        }

        [CombinedClass]
        [FormerInterface]
        public class VideoEncoder : dom.EventTarget
        {
            public extern VideoEncoder(dom.VideoEncoderInit init);
            public static dom.VideoEncoder prototype { get; set; }

            public static extern es5.Promise<dom.VideoEncoderSupport> isConfigSupported(dom.VideoEncoderConfig config);

            public virtual double encodeQueueSize { get; }
            public virtual dom.CodecState state { get; }

            public virtual extern void configure(dom.VideoEncoderConfig config);
            public virtual extern void encode(dom.VideoFrame frame);
            public virtual extern void encode(dom.VideoFrame frame, dom.VideoEncoderEncodeOptions options);
            public virtual extern void flush();
            public virtual extern void reset();
            public virtual extern void close();
        }

        [CombinedClass]
        [FormerInterface]
        public class AudioEncoder : dom.EventTarget
        {
            public extern AudioEncoder(dom.AudioEncoderInit init);
            public static dom.AudioEncoder prototype { get; set; }

            public static extern es5.Promise<dom.AudioEncoderSupport> isConfigSupported(dom.AudioEncoderConfig config);

            public virtual double encodeQueueSize { get; }
            public virtual dom.CodecState state { get; }

            public virtual extern void configure(dom.AudioEncoderConfig config);
            public virtual extern void encode(dom.AudioData data);
            public virtual extern void flush();
            public virtual extern void reset();
            public virtual extern void close();
        }

        [CombinedClass]
        [FormerInterface]
        public class VideoDecoder : dom.EventTarget
        {
            public extern VideoDecoder(dom.VideoDecoderInit init);
            public static dom.VideoDecoder prototype { get; set; }

            public static extern es5.Promise<dom.VideoDecoderSupport> isConfigSupported(dom.VideoDecoderConfig config);

            public virtual double decodeQueueSize { get; }
            public virtual dom.CodecState state { get; }

            public virtual extern void configure(dom.VideoDecoderConfig config);
            public virtual extern void decode(dom.EncodedVideoChunk chunk);
            public virtual extern void flush();
            public virtual extern void reset();
            public virtual extern void close();
        }

        [CombinedClass]
        [FormerInterface]
        public class AudioDecoder : dom.EventTarget
        {
            public extern AudioDecoder(dom.AudioDecoderInit init);
            public static dom.AudioDecoder prototype { get; set; }

            public static extern es5.Promise<dom.AudioDecoderSupport> isConfigSupported(dom.AudioDecoderConfig config);

            public virtual double decodeQueueSize { get; }
            public virtual dom.CodecState state { get; }

            public virtual extern void configure(dom.AudioDecoderConfig config);
            public virtual extern void decode(dom.EncodedAudioChunk chunk);
            public virtual extern void flush();
            public virtual extern void reset();
            public virtual extern void close();
        }

        [CombinedClass]
        [FormerInterface]
        public class EncodedVideoChunk : IObject
        {
            public extern EncodedVideoChunk(dom.EncodedVideoChunkInit init);
            public static dom.EncodedVideoChunk prototype { get; set; }

            public virtual dom.EncodedVideoChunkType type { get; }
            public virtual double timestamp { get; }
            public virtual double duration { get; }
            public virtual double byteLength { get; }

            public virtual extern void copyTo(es5.ArrayBufferView destination); // or BufferSource
        }

        [CombinedClass]
        [FormerInterface]
        public class EncodedAudioChunk : IObject
        {
            public extern EncodedAudioChunk(dom.EncodedAudioChunkInit init);
            public static dom.EncodedAudioChunk prototype { get; set; }

            public virtual dom.EncodedAudioChunkType type { get; }
            public virtual double timestamp { get; }
            public virtual double duration { get; }
            public virtual double byteLength { get; }

            public virtual extern void copyTo(es5.ArrayBufferView destination);
        }

        [CombinedClass]
        [FormerInterface]
        public class VideoFrame : IObject
        {
            public extern VideoFrame(dom.CanvasImageSource image);
            public extern VideoFrame(dom.CanvasImageSource image, dom.VideoFrameInit init);
            public extern VideoFrame(es5.ArrayBufferView data, dom.VideoFrameBufferInit init); // or BufferSource

            public static dom.VideoFrame prototype { get; set; }

            public virtual dom.VideoPixelFormat format { get; }
            public virtual double timestamp { get; }
            public virtual double duration { get; }
            public virtual double codedWidth { get; }
            public virtual double codedHeight { get; }
            public virtual dom.DOMRectReadOnly codedRect { get; }
            public virtual dom.DOMRectReadOnly visibleRect { get; }
            public virtual double displayWidth { get; }
            public virtual double displayHeight { get; }
            public virtual dom.VideoColorSpace colorSpace { get; }

            public virtual extern double allocationSize();
            public virtual extern double allocationSize(dom.VideoFrameCopyToOptions options);
            public virtual extern es5.Promise<dom.PlaneLayout[]> copyTo(es5.ArrayBufferView destination);
            public virtual extern es5.Promise<dom.PlaneLayout[]> copyTo(es5.ArrayBufferView destination, dom.VideoFrameCopyToOptions options);
            public virtual extern dom.VideoFrame clone();
            public virtual extern void close();
        }

        [CombinedClass]
        [FormerInterface]
        public class AudioData : IObject
        {
            public extern AudioData(dom.AudioDataInit init);
            public static dom.AudioData prototype { get; set; }

            public virtual dom.AudioSampleFormat format { get; }
            public virtual double sampleRate { get; }
            public virtual double numberOfFrames { get; }
            public virtual double numberOfChannels { get; }
            public virtual double duration { get; }
            public virtual double timestamp { get; }

            public virtual extern double allocationSize(dom.AudioDataCopyToOptions options);
            public virtual extern void copyTo(es5.ArrayBufferView destination, dom.AudioDataCopyToOptions options);
            public virtual extern dom.AudioData clone();
            public virtual extern void close();
        }

        // Init and Config Interfaces

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class VideoEncoderInit : IObject
        {
            public dom.WebCodecsErrorCallback error { get; set; }
            public dom.EncodedVideoChunkOutputCallback output { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class AudioEncoderInit : IObject
        {
            public dom.WebCodecsErrorCallback error { get; set; }
            public dom.EncodedAudioChunkOutputCallback output { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class VideoDecoderInit : IObject
        {
            public dom.WebCodecsErrorCallback error { get; set; }
            public dom.VideoFrameOutputCallback output { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class AudioDecoderInit : IObject
        {
            public dom.WebCodecsErrorCallback error { get; set; }
            public dom.AudioDataOutputCallback output { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class VideoEncoderConfig : IObject
        {
            public string codec { get; set; }
            public double width { get; set; }
            public double height { get; set; }
            public double? bitrate { get; set; }
            public double? framerate { get; set; }
            public dom.HardwareAcceleration hardwareAcceleration { get; set; }
            public dom.AlphaOption alpha { get; set; }
            public string scalabilityMode { get; set; }
            public dom.BitrateMode bitrateMode { get; set; }
            public dom.LatencyMode latencyMode { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class VideoEncoderEncodeOptions : IObject
        {
            public bool? keyFrame { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class AudioEncoderConfig : IObject
        {
            public string codec { get; set; }
            public double? sampleRate { get; set; }
            public double? numberOfChannels { get; set; }
            public double? bitrate { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class VideoDecoderConfig : IObject
        {
            public string codec { get; set; }
            public es5.ArrayBufferView description { get; set; }
            public double? codedWidth { get; set; }
            public double? codedHeight { get; set; }
            public double? displayAspectWidth { get; set; }
            public double? displayAspectHeight { get; set; }
            public dom.VideoColorSpaceInit colorSpace { get; set; }
            public dom.HardwareAcceleration hardwareAcceleration { get; set; }
            public bool? optimizeForLatency { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class AudioDecoderConfig : IObject
        {
            public string codec { get; set; }
            public double? sampleRate { get; set; }
            public double? numberOfChannels { get; set; }
            public es5.ArrayBufferView description { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class EncodedVideoChunkInit : IObject
        {
            public dom.EncodedVideoChunkType type { get; set; }
            public double timestamp { get; set; }
            public double? duration { get; set; }
            public es5.ArrayBufferView data { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class EncodedAudioChunkInit : IObject
        {
            public dom.EncodedAudioChunkType type { get; set; }
            public double timestamp { get; set; }
            public double? duration { get; set; }
            public es5.ArrayBufferView data { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class VideoFrameInit : IObject
        {
            public double? timestamp { get; set; }
            public double? duration { get; set; }
            public dom.AlphaOption alpha { get; set; }
            public dom.DOMRectInit visibleRect { get; set; }
            public double? displayWidth { get; set; }
            public double? displayHeight { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class VideoFrameBufferInit : IObject
        {
            public dom.VideoPixelFormat format { get; set; }
            public double codedWidth { get; set; }
            public double codedHeight { get; set; }
            public double timestamp { get; set; }
            public double? duration { get; set; }
            public dom.PlaneLayout[] layout { get; set; }
            public dom.DOMRectInit visibleRect { get; set; }
            public double? displayWidth { get; set; }
            public double? displayHeight { get; set; }
            public dom.VideoColorSpaceInit colorSpace { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class AudioDataInit : IObject
        {
            public dom.AudioSampleFormat format { get; set; }
            public double sampleRate { get; set; }
            public double numberOfFrames { get; set; }
            public double numberOfChannels { get; set; }
            public double timestamp { get; set; }
            public es5.ArrayBufferView data { get; set; }
            public dom.AudioDataCopyToOptions[] transfer { get; set; } // Note: MDN says transfer is array of ArrayBuffer
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class AudioDataCopyToOptions : IObject
        {
            public double planeIndex { get; set; }
            public double? frameOffset { get; set; }
            public double? frameCount { get; set; }
            public dom.AudioSampleFormat format { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class VideoFrameCopyToOptions : IObject
        {
            public dom.DOMRectInit rect { get; set; }
            public dom.PlaneLayout[] layout { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class PlaneLayout : IObject
        {
            public double offset { get; set; }
            public double stride { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class VideoEncoderSupport : IObject
        {
            public bool supported { get; set; }
            public dom.VideoEncoderConfig config { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class VideoDecoderSupport : IObject
        {
            public bool supported { get; set; }
            public dom.VideoDecoderConfig config { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class AudioEncoderSupport : IObject
        {
            public bool supported { get; set; }
            public dom.AudioEncoderConfig config { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class AudioDecoderSupport : IObject
        {
            public bool supported { get; set; }
            public dom.AudioDecoderConfig config { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class VideoColorSpaceInit : IObject
        {
            public dom.VideoColorPrimaries primaries { get; set; }
            public dom.VideoTransferCharacteristics transfer { get; set; }
            public dom.VideoMatrixCoefficients matrix { get; set; }
            public bool? fullRange { get; set; }
        }

        [CombinedClass]
        [FormerInterface]
        public class VideoColorSpace : IObject
        {
            public extern VideoColorSpace(dom.VideoColorSpaceInit init);
            public static dom.VideoColorSpace prototype { get; set; }

            public virtual dom.VideoColorPrimaries primaries { get; }
            public virtual dom.VideoTransferCharacteristics transfer { get; }
            public virtual dom.VideoMatrixCoefficients matrix { get; }
            public virtual bool? fullRange { get; }
            public virtual extern dom.VideoColorSpaceInit toJSON();
        }

        // Callbacks
        [Generated]
        public delegate void WebCodecsErrorCallback(dom.DOMException error);

        [Generated]
        public delegate void EncodedVideoChunkOutputCallback(dom.EncodedVideoChunk chunk, dom.EncodedVideoChunkMetadata metadata);

        [Generated]
        public delegate void EncodedAudioChunkOutputCallback(dom.EncodedAudioChunk chunk, dom.EncodedAudioChunkMetadata metadata);

        [Generated]
        public delegate void VideoFrameOutputCallback(dom.VideoFrame frame);

        [Generated]
        public delegate void AudioDataOutputCallback(dom.AudioData data);

        // Metadata Interfaces (Simplified)
        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class EncodedVideoChunkMetadata : IObject {
            public dom.VideoDecoderConfig decoderConfig { get; set; }
            // svc metadata etc.
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class EncodedAudioChunkMetadata : IObject {
            public dom.AudioDecoderConfig decoderConfig { get; set; }
        }

        // Literals
        [Name("System.String")]
        public class CodecState : LiteralType<string> {
            [Template("<self>\"unconfigured\"")] public static readonly dom.CodecState unconfigured;
            [Template("<self>\"configured\"")] public static readonly dom.CodecState configured;
            [Template("<self>\"closed\"")] public static readonly dom.CodecState closed;
            private extern CodecState();
            public static extern implicit operator dom.CodecState(string value);
        }

        [Name("System.String")]
        public class EncodedVideoChunkType : LiteralType<string> {
            [Template("<self>\"key\"")] public static readonly dom.EncodedVideoChunkType key;
            [Template("<self>\"delta\"")] public static readonly dom.EncodedVideoChunkType delta;
            private extern EncodedVideoChunkType();
            public static extern implicit operator dom.EncodedVideoChunkType(string value);
        }

        [Name("System.String")]
        public class EncodedAudioChunkType : LiteralType<string> {
            [Template("<self>\"key\"")] public static readonly dom.EncodedAudioChunkType key;
            [Template("<self>\"delta\"")] public static readonly dom.EncodedAudioChunkType delta;
            private extern EncodedAudioChunkType();
            public static extern implicit operator dom.EncodedAudioChunkType(string value);
        }

        [Name("System.String")]
        public class HardwareAcceleration : LiteralType<string> {
            [Template("<self>\"no-preference\"")] public static readonly dom.HardwareAcceleration noPreference;
            [Template("<self>\"prefer-hardware\"")] public static readonly dom.HardwareAcceleration preferHardware;
            [Template("<self>\"prefer-software\"")] public static readonly dom.HardwareAcceleration preferSoftware;
            private extern HardwareAcceleration();
            public static extern implicit operator dom.HardwareAcceleration(string value);
        }

        [Name("System.String")]
        public class AlphaOption : LiteralType<string> {
            [Template("<self>\"keep\"")] public static readonly dom.AlphaOption keep;
            [Template("<self>\"discard\"")] public static readonly dom.AlphaOption discard;
            private extern AlphaOption();
            public static extern implicit operator dom.AlphaOption(string value);
        }

        [Name("System.String")]
        public class BitrateMode : LiteralType<string> {
            [Template("<self>\"constant\"")] public static readonly dom.BitrateMode constant;
            [Template("<self>\"variable\"")] public static readonly dom.BitrateMode variable;
            [Template("<self>\"quantizer\"")] public static readonly dom.BitrateMode quantizer;
            private extern BitrateMode();
            public static extern implicit operator dom.BitrateMode(string value);
        }

        [Name("System.String")]
        public class LatencyMode : LiteralType<string> {
            [Template("<self>\"quality\"")] public static readonly dom.LatencyMode quality;
            [Template("<self>\"realtime\"")] public static readonly dom.LatencyMode realtime;
            private extern LatencyMode();
            public static extern implicit operator dom.LatencyMode(string value);
        }

        [Name("System.String")]
        public class VideoPixelFormat : LiteralType<string> {
            [Template("<self>\"I420\"")] public static readonly dom.VideoPixelFormat I420;
            [Template("<self>\"I420A\"")] public static readonly dom.VideoPixelFormat I420A;
            [Template("<self>\"I422\"")] public static readonly dom.VideoPixelFormat I422;
            [Template("<self>\"I444\"")] public static readonly dom.VideoPixelFormat I444;
            [Template("<self>\"NV12\"")] public static readonly dom.VideoPixelFormat NV12;
            [Template("<self>\"RGBA\"")] public static readonly dom.VideoPixelFormat RGBA;
            [Template("<self>\"RGBX\"")] public static readonly dom.VideoPixelFormat RGBX;
            [Template("<self>\"BGRA\"")] public static readonly dom.VideoPixelFormat BGRA;
            [Template("<self>\"BGRX\"")] public static readonly dom.VideoPixelFormat BGRX;
            private extern VideoPixelFormat();
            public static extern implicit operator dom.VideoPixelFormat(string value);
        }

        [Name("System.String")]
        public class AudioSampleFormat : LiteralType<string> {
            [Template("<self>\"u8\"")] public static readonly dom.AudioSampleFormat u8;
            [Template("<self>\"s16\"")] public static readonly dom.AudioSampleFormat s16;
            [Template("<self>\"s32\"")] public static readonly dom.AudioSampleFormat s32;
            [Template("<self>\"f32\"")] public static readonly dom.AudioSampleFormat f32;
            [Template("<self>\"u8-planar\"")] public static readonly dom.AudioSampleFormat u8Planar;
            [Template("<self>\"s16-planar\"")] public static readonly dom.AudioSampleFormat s16Planar;
            [Template("<self>\"s32-planar\"")] public static readonly dom.AudioSampleFormat s32Planar;
            [Template("<self>\"f32-planar\"")] public static readonly dom.AudioSampleFormat f32Planar;
            private extern AudioSampleFormat();
            public static extern implicit operator dom.AudioSampleFormat(string value);
        }

        [Name("System.String")]
        public class VideoColorPrimaries : LiteralType<string> {
            [Template("<self>\"bt709\"")] public static readonly dom.VideoColorPrimaries bt709;
            [Template("<self>\"bt470bg\"")] public static readonly dom.VideoColorPrimaries bt470bg;
            [Template("<self>\"smpte170m\"")] public static readonly dom.VideoColorPrimaries smpte170m;
            private extern VideoColorPrimaries();
            public static extern implicit operator dom.VideoColorPrimaries(string value);
        }

        [Name("System.String")]
        public class VideoTransferCharacteristics : LiteralType<string> {
            [Template("<self>\"bt709\"")] public static readonly dom.VideoTransferCharacteristics bt709;
            [Template("<self>\"smpte170m\"")] public static readonly dom.VideoTransferCharacteristics smpte170m;
            [Template("<self>\"iec61966-2-1\"")] public static readonly dom.VideoTransferCharacteristics iec61966_2_1;
            private extern VideoTransferCharacteristics();
            public static extern implicit operator dom.VideoTransferCharacteristics(string value);
        }

        [Name("System.String")]
        public class VideoMatrixCoefficients : LiteralType<string> {
            [Template("<self>\"rgb\"")] public static readonly dom.VideoMatrixCoefficients rgb;
            [Template("<self>\"bt709\"")] public static readonly dom.VideoMatrixCoefficients bt709;
            [Template("<self>\"bt470bg\"")] public static readonly dom.VideoMatrixCoefficients bt470bg;
            [Template("<self>\"smpte170m\"")] public static readonly dom.VideoMatrixCoefficients smpte170m;
            private extern VideoMatrixCoefficients();
            public static extern implicit operator dom.VideoMatrixCoefficients(string value);
        }

        [Virtual]
        public abstract class VideoEncoderTypeConfig : IObject { public virtual dom.VideoEncoder prototype { get; set; } }
        [Virtual]
        public abstract class AudioEncoderTypeConfig : IObject { public virtual dom.AudioEncoder prototype { get; set; } }
        [Virtual]
        public abstract class VideoDecoderTypeConfig : IObject { public virtual dom.VideoDecoder prototype { get; set; } }
        [Virtual]
        public abstract class AudioDecoderTypeConfig : IObject { public virtual dom.AudioDecoder prototype { get; set; } }
    }
}
