using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
namespace Random
{
    enum SortOrder { ID_ASC, ID_DESC,NAME_ASC,NAME_DESC }
    enum CheatMod { THROW_OUT, KICK_IN }
    class NameListReader
    {
        public Dictionary<int, CheatMod> CheatData = new Dictionary<int, CheatMod>(10);
        public Dictionary<int, string> StudentsList = new Dictionary<int, string>(100);
        private XmlDocument XMLFile;
         public NameListReader(string filePath) {
            LoadXML(filePath);
        }
        ~NameListReader() {
        }
        public void LoadXML(string filePath) {
            StudentsList.Clear();
            XMLFile = new XmlDocument();
            XMLFile.Load(filePath);
        }
        public void DoReadXML()
        {
            XmlElement StudentsElem = XMLFile.DocumentElement;
            XmlNodeList StudentNodes = StudentsElem.GetElementsByTagName("student");
            foreach (XmlNode studentObj in StudentNodes)
            {
                string studentName="";
                int studentID = int.Parse(((XmlElement)studentObj).GetAttribute("id"));
                XmlNodeList studentNameNode = ((XmlElement)studentObj).GetElementsByTagName("name");
                
                if (studentNameNode.Count == 1)
                {
                    studentName = studentNameNode[0].InnerText;
                }
                else
                {
                    throw (new Exception("Error: Student " + studentID + " have a wrong node count of name."));
                }
                
                StudentsList.Add(studentID, studentName);
            }
            XmlNodeList CD = StudentsElem.GetElementsByTagName("Fliter");
            DoReadCheatData(ref CD);
        }
        public void ListSort(SortOrder SO)
        {
            if(SO==SortOrder.ID_ASC)
                StudentsList  = (from objDic in StudentsList orderby objDic.Key select objDic).ToDictionary(pair => pair.Key,pair => pair.Value);
            else if(SO== SortOrder.ID_DESC)
                StudentsList = (from objDic in StudentsList orderby objDic.Key descending select objDic).ToDictionary(pair => pair.Key, pair => pair.Value);
            else if (SO == SortOrder.NAME_ASC)
                StudentsList = (from objDic in StudentsList orderby objDic.Value select objDic).ToDictionary(pair => pair.Key, pair => pair.Value);
            else if (SO == SortOrder.NAME_DESC)
                StudentsList = (from objDic in StudentsList orderby objDic.Value descending select objDic).ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        public void DoReadCheatData(ref XmlNodeList cheatNodes)
        {
            try
            {
                foreach (XmlNode cheatInfo in cheatNodes)
                {
                    int id_c = int.Parse(((XmlElement)cheatInfo).GetAttribute("id"));
                    if (StudentsList[id_c] == null)
                        throw (new Exception("Unknown error. NAME_OUT_OF_LIST"));
                    string cheatSrc = ((XmlElement)cheatInfo).GetAttribute("mod");
                    if (cheatSrc == "TO")
                        CheatData.Add(id_c, CheatMod.THROW_OUT);
                    else if (cheatSrc == "KI")
                        CheatData.Add(id_c, CheatMod.KICK_IN);
                    else
                        throw (new Exception("Unknown error. MOD_NOT_DEF"));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
   
}
