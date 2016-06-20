using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.logic
{
    /// <summary>
    /// AlgorithmTest
    /// </summary>
    public class AlgorithmTest
    {
        /// <summary>
        /// bidId
        /// </summary>
        public static List<string> randomValues = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        /// <summary>
        /// 递归实例练习
        /// </summary>
        /// <param name="curnumber"></param>
        public void Recursion(string curnumber)
        {
            if (string.IsNullOrEmpty(curnumber) || curnumber == "ZZZZ")
            {

            }
            else
            {
                string temp = curnumber[curnumber.Length - 1].ToString();
                int position = randomValues.IndexOf(temp);// 根据值着索引位置
                if (randomValues[randomValues.Count - 1] == temp)
                {
                    Recursion(curnumber.Substring(0, curnumber.Length - 1)) + randomValues[0];// 如果最后一个字符已经为最大值 则递归他前面的字符并且把他的值变成最小值。
                }
                else
                {
                    curnumber.Substring(0, curnumber.Length - 1).ToString() + randomValues[position + 1];
                }
            }
        }
    }
}
