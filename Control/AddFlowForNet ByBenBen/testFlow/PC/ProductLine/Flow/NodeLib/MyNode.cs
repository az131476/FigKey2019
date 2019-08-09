using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PC.ProductLine.Flow.NodeLib
{
    public class MyNode : Lassalle.Flow.Node
    {
        private string m_author = null;
        private string m_comment = null;

        public MyNode(Lassalle.Flow.DefNode defnode, string author, string comment) : base(defnode)
        {
            m_author = author;
            m_comment = comment;
        }

        public string Author
        {
            get { return m_author; }
            set { m_author = value; }
        }

        public string Comment
        {
            get { return m_comment; }
            set { m_comment = value; }
        }
    }
}
