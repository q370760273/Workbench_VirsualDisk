using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualDisk
{
    public struct MString
    {
        public static readonly MString Null = null;
        public static readonly MString Empty = "";
        private char[] _data;

        public MString(string str)
        {
            if(str == null)
                throw new Exception("cannot initial MString by null!");

            _data = str.ToArray();
        }

        public MString(char[] data)
        {
            if (data == null)
                throw new Exception("cannot initial MString by null!");

            _data = data;
        }

        public static implicit operator MString (string str)
        {
            if (str == null)
                return default(MString);

            return new MString(str);
        }

        public static implicit operator string(MString mStr)
        {
            return new string(mStr._data);
        }

        public static MString operator +(MString a, MString b)
        {
            if (a == Null)
                return b;
            if (b == Null)
                return a;

            int aLength = a.Length;
            int bLength = b.Length;
            char[] newData = new char[aLength + bLength];
            Array.Copy(a._data, newData, aLength);
            Array.Copy(b._data, 0, newData, aLength, bLength);
            return new MString(newData);
        }

        public static bool operator ==(MString a, MString b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(MString a, MString b)
        {
            return !Equals(a, b);
        }

        public static bool Equals(MString a, MString b)
        {
            int aH = a.GetHashCode();
            int bH = b.GetHashCode();
            if (aH == bH)
            {
                return true;
            }
            if ((aH == Null.GetHashCode()) || (bH == Null.GetHashCode()))
            {
                return false;
            }
            return EqualsHelper(a, b);
        }

        private static bool EqualsHelper(MString strA, MString strB)
        {
            if (strA.Length != strB.Length)
            {
                return false;
            }

            int i = 0;
            while (i < strA.Length)
            {
                if (strA[i] != strB[i])
                    return false;

                i++;
            }
            return true;
        }

        public char this[int index]
        {
            get { return _data[index]; }
        }

        public int Length
        { get { return _data.Length; } }

        public MString Replace(string oldStr, string newStr)
        {
            if (string.IsNullOrEmpty(oldStr))
                throw new Exception("match string cannot is null or empty!");

            int oldStrLen = oldStr.Length;
            char[] oldChars = oldStr.ToArray();
            Queue<int> recordQueue = new Queue<int>();
            int i, j;
            for (i = 0; i <= _data.Length - oldStrLen; i++)
            {
                if (_data[i] == oldChars[0])
                {
                    bool matchSucceed = true;
                    for (j = 1; j < oldStrLen; j++)
                    {
                        if (_data[i + j] != oldChars[j])
                        {
                            matchSucceed = false;
                            continue;
                        }
                    }

                    if (matchSucceed)
                    {
                        recordQueue.Enqueue(i);
                        i += oldStrLen - 1;
                    }
                }
            }

            if (recordQueue.Count == 0)
                return this;


            int newStrLen = newStr.Length;
            char[] newChars = newStr.ToArray();
            char[] data = new char[Length - recordQueue.Count * (oldStrLen - newStrLen)];

            int removeIndex = recordQueue.Dequeue();
            int writeIndex = 0;
            int readIndex = 0;
            while (writeIndex < data.Length)
            {
                if (readIndex < removeIndex)
                {
                    data[writeIndex++] = _data[readIndex++];
                }
                else
                {
                    for (i = 0; i < newStrLen; i++)
                    {
                        data[writeIndex++] = newChars[i];
                    }
                    readIndex += oldStrLen;

                    if (recordQueue.Count > 0)
                        removeIndex = recordQueue.Dequeue();
                    else
                        removeIndex = int.MaxValue;
                }
            }
            return new MString(data);
        }

        public MString MultiReplace(string oldStr, string newStr)
        {
            if (string.IsNullOrEmpty(oldStr))
                throw new Exception("match string cannot is null or empty!");

            int oldStrLen = oldStr.Length;
            char[] oldChars = oldStr.ToArray();
            List<KeyValuePair<int, int>> recordList = new List<KeyValuePair<int, int>>();
            int i, j;
            for (i = 0; i <= _data.Length - oldStrLen; i++)
            {
                if (_data[i] == oldChars[0])
                {
                    bool matchSucceed = true;
                    for (j = 1; j < oldStrLen; j++)
                    {
                        if (_data[i + j] != oldChars[j])
                        {
                            matchSucceed = false;
                            continue;
                        }
                    }

                    if (matchSucceed)
                    {
                        if (recordList.Count == 0)
                        {
                            recordList.Add(new KeyValuePair<int, int>(i, oldStrLen));
                        }
                        else
                        {
                            KeyValuePair<int, int> lastItem = recordList[recordList.Count - 1];
                            if (lastItem.Key + lastItem.Value >= i)
                            {
                                recordList[recordList.Count - 1] = new KeyValuePair<int, int>(lastItem.Key, lastItem.Value + oldStrLen);
                            }
                            else
                            {
                                recordList.Add(new KeyValuePair<int, int>(i, oldStrLen));
                            }
                        }
                        i += oldStrLen - 1;
                    }
                }
            }

            if (recordList.Count == 0)
                return this;

            

            int newStrLen = newStr.Length;
            char[] newChars = newStr.ToArray();
            int reduceCount = 0;
            for (i = 0; i < recordList.Count; i++)
            {
                reduceCount += (recordList[i].Value - newStrLen);
            }

            char[] data = new char[Length - reduceCount];
            int readRecordListIndex = 0;
            int removeIndex = recordList[readRecordListIndex].Key;
            int writeIndex = 0;
            int readIndex = 0;
            while (writeIndex < data.Length)
            {
                if (readIndex < removeIndex)
                {
                    data[writeIndex++] = _data[readIndex++];
                }
                else
                {
                    for (i = 0; i < newStrLen; i++)
                    {
                        data[writeIndex++] = newChars[i];
                    }
                    readIndex += recordList[readRecordListIndex].Value;

                    if (++readRecordListIndex < recordList.Count)
                        removeIndex = recordList[readRecordListIndex].Key;
                    else
                        removeIndex = int.MaxValue;
                }
            }
            return new MString(data);
        }

        public MString Substring(int startIndex)
        {
            return this.Substring(startIndex, this.Length - startIndex);
        }

        public MString Substring(int startIndex, int length)
        {
            if (startIndex < 0 || startIndex > this.Length || length < 0 || startIndex > (this.Length - length))
            {
                throw new Exception("startIndex or length is out of range!");
            }
            if (length == 0)
            {
                return Empty;
            }
            if ((startIndex == 0) && (length == this.Length))
            {
                return this;
            }

            char[] data = new char[length];
            Array.Copy(_data, startIndex, data, 0, length);
            return new MString(data);
        }

        public MString Trim(params char[] trimChars)
        {
            if ((trimChars != null) && (trimChars.Length != 0))
            {
                return this.TrimHelper(trimChars, 2);
            }
            return this.TrimHelper(2);
        }

        public MString TrimStart(params char[] trimChars)
        {
            if ((trimChars != null) && (trimChars.Length != 0))
            {
                return this.TrimHelper(trimChars, 0);
            }
            return this.TrimHelper(0);
        }

        public MString TrimEnd(params char[] trimChars)
        {
            if ((trimChars != null) && (trimChars.Length != 0))
            {
                return this.TrimHelper(trimChars, 1);
            }
            return this.TrimHelper(1);
        }

        private MString TrimHelper(int trimType)
        {
            int end = this.Length - 1;
            int start = 0;
            if (trimType != 1)
            {
                start = 0;
                while (start < this.Length)
                {
                    if (!char.IsWhiteSpace(this[start]))
                    {
                        break;
                    }
                    start++;
                }
            }
            if (trimType != 0)
            {
                end = this.Length - 1;
                while (end >= start)
                {
                    if (!char.IsWhiteSpace(this[end]))
                    {
                        break;
                    }
                    end--;
                }
            }
            return this.CreateTrimmedString(start, end);
        }

        private MString TrimHelper(char[] trimChars, int trimType)
        {
            int end = this.Length - 1;
            int start = 0;
            if (trimType != 1)
            {
                start = 0;
                while (start < this.Length)
                {
                    int index = 0;
                    char ch = this[start];
                    index = 0;
                    while (index < trimChars.Length)
                    {
                        if (trimChars[index] == ch)
                        {
                            break;
                        }
                        index++;
                    }
                    if (index == trimChars.Length)
                    {
                        break;
                    }
                    start++;
                }
            }
            if (trimType != 0)
            {
                end = this.Length - 1;
                while (end >= start)
                {
                    int num4 = 0;
                    char ch2 = this[end];
                    num4 = 0;
                    while (num4 < trimChars.Length)
                    {
                        if (trimChars[num4] == ch2)
                        {
                            break;
                        }
                        num4++;
                    }
                    if (num4 == trimChars.Length)
                    {
                        break;
                    }
                    end--;
                }
            }
            return this.CreateTrimmedString(start, end);
        }

        private MString CreateTrimmedString(int start, int end)
        {
            int length = (end - start) + 1;
            if (length == this.Length)
            {
                return this;
            }
            if (length == 0)
            {
                return Empty;
            }
            return Substring(start, length);
        }

        public bool StartsWith(string value, bool ignoreCase = false)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return CultureInfo.CurrentCulture.CompareInfo.IsPrefix(this, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        }

        public bool StartsWith(string[] values, bool ignoreCase = false)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            foreach (string val in values)
            {
                if (val == null)
                    throw new ArgumentNullException("value");

                if (CultureInfo.CurrentCulture.CompareInfo.IsPrefix(this, val, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None))
                    return true;
            }

            return false;
        }

        public bool EndsWith(string value, bool ignoreCase = false)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return CultureInfo.CurrentCulture.CompareInfo.IsSuffix(this, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        }

        public int IndexOf(string value, bool ignoreCase = false)
        {
            return IndexOf(value, 0, Length, ignoreCase);
        }

        public int IndexOf(string value, int startIndex, bool ignoreCase = false)
        {
            return IndexOf(value, startIndex, Length - startIndex, ignoreCase);
        }

        public int IndexOf(string value, int startIndex, int count, bool ignoreCase = false)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if ((startIndex < 0) || (startIndex > this.Length) || (count < 0) || (startIndex > (this.Length - count)))
            {
                throw new Exception("startIndex or length is out of range!");
            }

            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, value, startIndex, count, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        }

        public int IndexOf(string[] values, bool ignoreCase = false)
        {
            return IndexOf(values, 0, Length, ignoreCase);
        }

        public int IndexOf(string[] values, int startIndex, bool ignoreCase = false)
        {
            return IndexOf(values, startIndex, Length - startIndex, ignoreCase);
        }

        public int IndexOf(string[] values, int startIndex, int count, bool ignoreCase = false)
        {
            if (values == null)
            {
                throw new ArgumentNullException("value");
            }
            if ((startIndex < 0) || (startIndex > this.Length) || (count < 0) || (startIndex > (this.Length - count)))
            {
                throw new Exception("startIndex or length is out of range!");
            }

            int min = int.MaxValue;
            foreach (string val in values)
            {
                if (val == null)
                    throw new ArgumentNullException("value");

                int index = CultureInfo.CurrentCulture.CompareInfo.IndexOf(this, val, startIndex, count, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);

                if (index != -1 && index < min)
                    min = index;
            }

            return min != int.MaxValue ? min : -1;
        }

        public MString Remove(int startIndex)
        {
            return Remove(startIndex, Length - startIndex);
        }

        public MString Remove(int startIndex, int count)
        {
            if ((startIndex < 0) || (startIndex > this.Length) || (count < 0) || (startIndex > (this.Length - count)))
            {
                throw new Exception("startIndex or length is out of range!");
            }

            int length = this.Length - count;
            if (length == 0)
            {
                return Empty;
            }

            char[] data = new char[length];
            if (startIndex != 0)
            {
                Array.Copy(_data, data, startIndex);
            }
            Array.Copy(_data, startIndex + count, data, startIndex, length - startIndex);
            return new MString(data);
        }

        public MString[] Split(params char[] separator)
        {
            Queue<MString> results = new Queue<MString>(); 
            int startIndex = 0;
            for (int i = 0; i < _data.Length; i++)
            {
                bool isSp = false;
                for(int j = 0; j < separator.Length; j++)
                {
                    if (separator[j] == _data[i])
                    {
                        isSp = true;
                        break;
                    }
                }

                if (isSp)
                {
                    results.Enqueue(Substring(startIndex, i - startIndex));
                    startIndex = i + 1;
                }
            }
            
            results.Enqueue(Substring(startIndex, _data.Length - startIndex));

            return results.ToArray();
        }

        public MString[] MultiSplit(params char[] separator)
        {
            Queue<MString> results = new Queue<MString>();
            int lastSpIndex = int.MaxValue;
            int startIndex = 0;
            for (int i = 0; i < _data.Length; i++)
            {
                bool isSp = false;
                for (int j = 0; j < separator.Length; j++)
                {
                    if (separator[j] == _data[i])
                    {
                        isSp = true;
                        break;
                    }
                }

                if (isSp)
                {
                    if (lastSpIndex + 1 != i)
                    {
                        results.Enqueue(Substring(startIndex, i - startIndex));
                    }

                    lastSpIndex = i;
                    startIndex = i + 1;
                }
            }

            results.Enqueue(Substring(startIndex, _data.Length - startIndex));

            return results.ToArray();
        }

        public MString ToLower()
        {
            return this.ToLower(CultureInfo.CurrentCulture);
        }

        public MString ToLower(CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }
            return culture.TextInfo.ToLower(this);
        }

        public MString ToLowerInvariant()
        {
            return this.ToLower(CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return this;
        }

        public MString ToUpper()
        {
            return this.ToUpper(CultureInfo.CurrentCulture);
        }

        public MString ToUpper(CultureInfo culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }
            return culture.TextInfo.ToUpper(this);
        }

        public MString ToUpperInvariant()
        {
            return this.ToUpper(CultureInfo.InvariantCulture);
        }

        public char Last()
        {
            if(Length == 0)
                throw new Exception("string is null or empty!");

            return _data[Length - 1];
        }
    }
}
