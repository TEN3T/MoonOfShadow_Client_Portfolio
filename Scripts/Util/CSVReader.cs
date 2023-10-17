using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };
    const string TABLE_HOME = "Table/";

    private static Dictionary<string, Dictionary<string, Dictionary<string, object>>> cachedTable = new Dictionary<string, Dictionary<string, Dictionary<string, object>>>();

    public static object Read(string file, string row, string col)
    {
        Dictionary<string, Dictionary<string, object>> dataDic = Read(file);
        if (dataDic.ContainsKey(row))
        {
            Dictionary<string, object> dataDicRow = dataDic[row];
            if (dataDicRow.ContainsKey(col))
            {
                return dataDicRow[col];
            }
        }

        DebugManager.Instance.PrintDebug("[Read] {0} 데이터를 찾을 수 없습니다.", col);
        return null;
    }

    public static Dictionary<string, Dictionary<string, object>> Read(string file)
    {
        Dictionary<string, Dictionary<string, object>> dataDic = new Dictionary<string, Dictionary<string, object>>();
        DebugManager.Instance.PrintDebug("[Read] CSVReader Called");

        if (cachedTable.ContainsKey(file))
        {
            DebugManager.Instance.PrintDebug("[Read] 캐시 데이터 사용");
            dataDic = cachedTable[file];
        }

        else
        {
            DebugManager.Instance.PrintDebug("[Read] 신규 데이터 로드 사용");
            TextAsset data = Resources.Load(TABLE_HOME + file) as TextAsset;

            if (data == null)
            {
                Debug.LogErrorFormat("[Read] {0} 파일을 찾을 수 없습니다.", TABLE_HOME + file);
                return null;
            }

            var lines = Regex.Split(data.text, LINE_SPLIT_RE);

            if (lines.Length <= 3)
            {
                Debug.LogErrorFormat("[Read] 해당 파일의 내용이 부족합니다.");
                return null;
            }


            string[] h = Regex.Split(lines[0], SPLIT_RE);

            for (int i = 3; i < lines.Length; i++)
            {
                string[] val = Regex.Split(lines[i], SPLIT_RE);

                Dictionary<string, object> _dataList = new Dictionary<string, object>();
                for (int j = 1; j < val.Length; j++)
                {
                    //_dataList.Add(h[j],val[j]);
                    if (val[j].Contains("|"))
                    {
                        List<string> list = Regex.Split(val[j], "[|]").Where(s => !string.IsNullOrEmpty(s)).ToList();
                        _dataList.Add(h[j], list);
                    }
                    else
                    {
                        _dataList.Add(h[j], val[j]);
                    }
                }

                dataDic.Add(val[0], _dataList);
            }
            cachedTable.Add(file,dataDic);

        }
        return dataDic;
    }
}


//public static List<Dictionary<string, object>> Read(string file)
//{
//	TextAsset data = Resources.Load(file) as TextAsset;

//	if (data == null)
//       {
//		Debug.LogErrorFormat("[Read] {0} 파일을 찾을 수 없습니다.", file);
//		return null;
//       }

//	var lines = Regex.Split(data.text, LINE_SPLIT_RE);

//	if (lines.Length <= 3)
//	{
//		Debug.LogErrorFormat("[Read] 해당 파일의 내용이 부족합니다.");
//		return null;
//	}

//       var list = new List<Dictionary<string, object>>();

//       var header = Regex.Split(lines[0], SPLIT_RE);
//       for (var i = 4; i < lines.Length; i++)
//       {
//           var values = Regex.Split(lines[i], SPLIT_RE);

//           if (values.Length == 0 || values[0] == "")
//           {
//               continue;
//           }

//           var entry = new Dictionary<string, object>();

//           for (var j = 0; j < header.Length && j < values.Length; j++)
//           {
//               string value = values[j].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
//               object finalvalue = value;
//               int n;
//               float f;

//               if (int.TryParse(value, out n))
//               {
//                   finalvalue = n;
//               }
//               else if (float.TryParse(value, out f))
//               {
//                   finalvalue = f;
//               }

//               entry[header[j]] = finalvalue;
//           }

//           list.Add(entry);
//       }

//       return list;
//   }
//   public static Dictionary<string, object> FindRead(string file, string header, object value)
//   {
//       var data = Resources.Load(file) as TextAsset;

//       if (data == null)
//       {
//           Debug.LogErrorFormat("[FindRead] {0} 파일을 찾을 수 없습니다.", file);
//           return null;
//       }

//       string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

//       if (lines.Length <= 3)
//       {
//           Debug.LogErrorFormat("[FindRead] 해당 파일의 내용이 부족합니다.");
//           return null;
//       }

//       string[] _header = Regex.Split(lines[0], SPLIT_RE);
//       string[] dataType = Regex.Split(lines[2], SPLIT_RE);
//       bool isFound = false;

//       for (int i = 3; i < lines.Length; i++)
//       {
//           string[] values = Regex.Split(lines[i], SPLIT_RE);

//           if (values.Length == 0 || string.IsNullOrEmpty(values[0]))
//           {
//               continue;
//           }

//           var entry = new Dictionary<string, object>();

//           for (int j = 0; j < _header.Length && j < values.Length; j++)
//           {
//               string _value = values[j].TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
//               object finalValue = _value;
//               int n;
//               float f;

//               if (dataType[j].Equals("Int_Arr"))
//               {
//                   string[] arrValue = Regex.Split(_value, ",");

//                   if (arrValue.Length > 0)
//                   {
//                       int[] iArr = new int[arrValue.Length];

//                       for (int k = 0; k < arrValue.Length; k++)
//                       {
//                           iArr[k] = int.Parse(arrValue[k]);
//                       }

//                       finalValue = iArr;
//                   }
//               }
//               else if (dataType[j].Equals("Int") && int.TryParse(_value, out n))
//               {
//                   finalValue = n;
//               }
//               else if (dataType[j].Equals("Float") && float.TryParse(_value, out f))
//               {
//                   finalValue = f;
//               }

//               if (_header[j] == header && finalValue.ToString() == value.ToString())
//               {
//                   isFound = true;
//               }

//               entry[_header[j]] = finalValue;
//           }

//           if (isFound)
//           {
//               return entry;
//           }
//       }

//       return null;
//   }
//   public static Dictionary<int, List<object>> FileRead(string file )
//   {
//       TextAsset data = Resources.Load(file) as TextAsset;

//       if (data == null)
//       {
//           Debug.LogErrorFormat("[Read] {0} 파일을 찾을 수 없습니다.", file);
//           return null;
//       }

//       var lines = Regex.Split(data.text, LINE_SPLIT_RE);

//       if (lines.Length <= 3)
//       {
//           Debug.LogErrorFormat("[Read] 해당 파일의 내용이 부족합니다.");
//           return null;
//       }

//       Dictionary<int, List<object>> dataDic = new Dictionary<int, List<object>>();
//       string[] h = Regex.Split(lines[0], SPLIT_RE);
//       for (int i = 3; i < lines.Length; i++)
//       {
//           string[] val = Regex.Split(lines[i], SPLIT_RE);

//           List<object> _dataList = new List<object>();
//           for (int j = 1; j < val.Length; j++)
//           {
//               _dataList.Add(val[j]);
//           }

//           dataDic.Add(int.Parse(val[0]), _dataList);
//       }

//       return dataDic;
//   }
