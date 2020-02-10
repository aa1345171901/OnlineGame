using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Model
{
   public class UserData
    {
        public UserData(string userData)
        {
            string[] strs = userData.Split(',');
            this.Id = int.Parse(strs[0]);
            this.userName = strs[1];
            this.totalCount = int.Parse(strs[2]);
            this.winCount = int.Parse(strs[3]);
        }

        public UserData(string userName,int totalCount,int winCount)
        {
            this.userName = userName;
            this.totalCount = totalCount;
            this.winCount = winCount;
        }

        public UserData(int id,string userName, int totalCount, int winCount)
        {
            this.Id = id;
            this.userName = userName;
            this.totalCount = totalCount;
            this.winCount = winCount;
        }

        public int Id { get;private set; }
        public  string userName { get; private set; }
        public int totalCount { get;  set; }
        public int winCount { get;  set; }
    }
}
