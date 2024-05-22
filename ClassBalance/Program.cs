using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using USAlgo.ClassBalance;

namespace ClassBalance
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Fenban fenban = new Fenban();

            // Excel文件本地路径
            string filePath = @".\TestData\100_Data.xlsx"; 
            // 分班设置 
            CalcSetting calcSetting = new CalcSetting
            {
                NumberClass = 4, //分班个数
                ColumnTypes = new ColumnTypeEnum[] { 
                    ColumnTypeEnum.Ignore, //保留列：不参与计算
                    ColumnTypeEnum.Name,  //名字列：字符串，不能为空
                    ColumnTypeEnum.Gender, //性别列：字符串，取值男或女，不能为空
                    ColumnTypeEnum.Score, ColumnTypeEnum.Score, ColumnTypeEnum.Score, //分数列：整数或浮点型（<=2位小数），不能为空
                    ColumnTypeEnum.Preset, //预设班级列: 字符串，可为空, 值为数字(如1/2/3)会分到此班级, 值为字母(如a/b/c)且字母相同的会分到一个班级
                    ColumnTypeEnum.Binding, //绑定列: 数字，相同数值代表必须在同一班级，可为空
                    ColumnTypeEnum.Mutex, //互斥列: 数字，相同数值代表不能在同一班级，可为空
                    ColumnTypeEnum.Balance, ColumnTypeEnum.Balance, ColumnTypeEnum.Balance // 均衡列：字符串，取值是，可为空
                },
                TopN = 10, //前n名均衡分配，0代表不启用
                LastN = 10, //后n名均衡分配，0代表不启用
                ScorePartition = 10, //分数分段统计均衡，适用于总分, 默认为10
                IsHomophonic = true, //是否开启同音同名分开, 默认开启
                FuzzyPinyin = new Dictionary<string, string>() //是否开启模糊音分开及模糊音定义
                {
                    { "s", "sh"},
                    { "c", "ch"},
                    { "z", "zh"},
                    { "f", "h"},
                    { "l", "n"},
                    { "r", "l"},
                    { "an", "ang"},
                    { "in", "ing"},
                    { "uan", "uang"},
                    { "en", "eng"},
                    { "ian", "iang"}
                }
            };

            // 基于数据量，计算时间1秒到120秒不等，请耐心等等
            var output = fenban.Calc(filePath, calcSetting);

            // 学生人数小于等于100时候，无需授权码。学生人数大于100时候，需要联系作者获取授权码并做为参数传入
            //var output1 = fenban.Calc(filePath, calcSetting, user: "", authorizationCode: "");

            Console.WriteLine($"描述信息: {output.CalcDescription}");
            Console.WriteLine($"计算结果: {output.IsSuccessed}");
            Console.WriteLine($"返回信息: {output.Message}");
            Console.WriteLine($"计算用时: {output.CalcElapsedTime}秒");
            Console.WriteLine($"详细结果: ");
            // 输出结果包括统计数据、各班详细数据、预设班级分配数据、同音同名模糊音分配数据、平衡列分配数据、分数段分配数据
            Console.WriteLine($"{JsonConvert.SerializeObject(output)}");
        }
    }
}
