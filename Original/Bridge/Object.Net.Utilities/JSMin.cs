// Decompiled with JetBrains decompiler
// Type: Object.Net.Utilities.JSMin
// Assembly: Object.Net.Utilities, Version=2.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0B5927F8-8BAA-4C4A-84B2-E1600C4FD3FC
// Assembly location: C:\bridge\2020-04-30\bridge\packages\Object.NET.Utilities.2.5.0\lib\Object.Net.Utilities.dll

using System;
using System.IO;

namespace Object.Net.Utilities
{
  internal class JSMin
  {
    private int theLookahead = -1;
    private const int EOF = -1;
    private StreamReader sr;
    private StreamWriter sw;
    private int theA;
    private int theB;

    public void Minify(StreamReader[] readers, string dst)
    {
      this.sw = new StreamWriter(dst);
      for (int index = 0; index < readers.Length; ++index)
      {
        using (this.sr = readers[index])
          this.jsmin();
      }
      this.sw.Close();
    }

    public void Minify(StreamReader reader, StreamWriter writer)
    {
      using (this.sr = reader)
      {
        using (this.sw = writer)
          this.jsmin();
      }
    }

    public void Minify(string instance, string dst)
    {
      this.Minify(new StreamReader(instance), new StreamWriter(dst));
    }

    private void jsmin()
    {
      this.theA = 10;
      this.action(3);
      while (this.theA != -1)
      {
        switch (this.theA)
        {
          case 10:
            switch (this.theB)
            {
              case 32:
                this.action(3);
                continue;
              case 40:
              case 43:
              case 45:
              case 91:
              case 123:
                this.action(1);
                continue;
              default:
                if (this.isAlphanum(this.theB))
                {
                  this.action(1);
                  continue;
                }
                this.action(2);
                continue;
            }
          case 32:
            if (this.isAlphanum(this.theB))
            {
              this.action(1);
              continue;
            }
            this.action(2);
            continue;
          default:
            switch (this.theB)
            {
              case 10:
                switch (this.theA)
                {
                  case 34:
                  case 39:
                  case 41:
                  case 43:
                  case 45:
                  case 93:
                  case 125:
                    this.action(1);
                    continue;
                  default:
                    if (this.isAlphanum(this.theA))
                    {
                      this.action(1);
                      continue;
                    }
                    this.action(3);
                    continue;
                }
              case 32:
                if (this.isAlphanum(this.theA))
                {
                  this.action(1);
                  continue;
                }
                this.action(3);
                continue;
              default:
                this.action(1);
                continue;
            }
        }
      }
    }

    private void action(int d)
    {
      if (d <= 1)
        this.put(this.theA);
      if (d <= 2)
      {
        this.theA = this.theB;
        if (this.theA == 39 || this.theA == 34)
        {
          while (true)
          {
            do
            {
              this.put(this.theA);
              this.theA = this.get();
              if (this.theA != this.theB)
              {
                if (this.theA <= 10)
                  throw new Exception(string.Format("Error: JSMIN unterminated string literal: {0}\n", (object) this.theA));
              }
              else
                goto label_9;
            }
            while (this.theA != 92);
            this.put(this.theA);
            this.theA = this.get();
          }
        }
      }
label_9:
      if (d > 3)
        return;
      this.theB = this.next();
      if (this.theB != 47 || this.theA != 40 && this.theA != 44 && (this.theA != 61 && this.theA != 91) && (this.theA != 33 && this.theA != 58 && (this.theA != 38 && this.theA != 124)) && (this.theA != 63 && this.theA != 123 && (this.theA != 125 && this.theA != 59) && this.theA != 10))
        return;
      this.put(this.theA);
      this.put(this.theB);
      while (true)
      {
        this.theA = this.get();
        if (this.theA != 47)
        {
          if (this.theA == 92)
          {
            this.put(this.theA);
            this.theA = this.get();
          }
          else if (this.theA <= 10)
            break;
          this.put(this.theA);
        }
        else
          goto label_18;
      }
      throw new Exception(string.Format("Error: JSMIN unterminated Regular Expression literal : {0}.\n", (object) this.theA));
label_18:
      this.theB = this.next();
    }

    private int next()
    {
      int num1 = this.get();
      if (num1 != 47)
        return num1;
      switch (this.peek())
      {
        case 42:
          this.get();
          int num2;
          do
          {
            num2 = this.get();
            if (num2 == -1)
              goto label_8;
          }
          while (num2 != 42 || this.peek() != 47);
          this.get();
          return 32;
label_8:
          throw new Exception("Error: JSMIN Unterminated comment.\n");
        case 47:
          int num3;
          do
          {
            num3 = this.get();
          }
          while (num3 > 10);
          return num3;
        default:
          return num1;
      }
    }

    private int peek()
    {
      this.theLookahead = this.get();
      return this.theLookahead;
    }

    private int get()
    {
      int num = this.theLookahead;
      this.theLookahead = -1;
      if (num == -1)
        num = this.sr.Read();
      if (num < 32)
      {
        switch (num)
        {
          case -1:
          case 10:
            break;
          case 13:
            return 10;
          default:
            return 32;
        }
      }
      return num;
    }

    private void put(int c)
    {
      this.sw.Write((char) c);
    }

    private bool isAlphanum(int c)
    {
      return c >= 97 && c <= 122 || c >= 48 && c <= 57 || (c >= 65 && c <= 90 || (c == 95 || c == 36)) || c == 92 || c > 126;
    }
  }
}
