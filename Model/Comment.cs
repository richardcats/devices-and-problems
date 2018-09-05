using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Comment 
    {

        public int CommentID
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }
    }
}
