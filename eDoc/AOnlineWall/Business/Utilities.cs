using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOnlineWall.Business
{
    public static class Utilities
    {
        //".pdf" 客服端没有装,在线看不起来   ".txt" 在浏览器里面看会会 乱码.   保存的方式是ASNI
        public static List<string> AOnlineUrlExtension = new List<string>() { ".txt", ".jpg", ".jpeg", ".gif", ".png", ".html"};

        public static List<string> AOnlineOpenExtension = new List<string>() { ".xlsx", ".xls", ".doc", ".docx", ".msg" };


    }
}
