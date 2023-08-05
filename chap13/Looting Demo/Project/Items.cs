using System;
using System.Collections.Generic;
using System.Xml;

namespace RPG
{
public class Items
{
    //keep public for easy access
    public List<Item> items;

    public Items() 
    {
        items = new List<Item>();
    }

    private string getElement(string field, ref XmlElement element)
    {
        string value = "";
        try
        {
            value = element.GetElementsByTagName(field)[0].InnerText;
        }
        catch (Exception){}
        return value;
    }

    public bool Load(string filename)
    {
        try
        {
            //open the xml file 
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNodeList list = doc.GetElementsByTagName("item");
            foreach (XmlNode node in list)
            {
                //get next item in table
                XmlElement element = (XmlElement)node;
                Item item = new Item();

                //store fields in new Item
                item.Name = getElement("name", ref element);
                item.Description = getElement("description", ref element);
                item.DropImageFilename = getElement("dropimagefilename", ref element);
                item.InvImageFilename = getElement("invimagefilename", ref element);
                item.Category = getElement("category", ref element);
                item.Weight = Convert.ToSingle(getElement("weight", ref element));
                item.Value = Convert.ToSingle(getElement("value", ref element));
                item.AttackNumDice = Convert.ToInt32(getElement("attacknumdice", ref element));
                item.AttackDie = Convert.ToInt32(getElement("attackdie", ref element));
                item.Defense = Convert.ToInt32(getElement("defense", ref element));
                item.STR = Convert.ToInt32(getElement("STR", ref element));
                item.DEX = Convert.ToInt32(getElement("DEX", ref element));
                item.STA = Convert.ToInt32(getElement("STA", ref element));
                item.INT = Convert.ToInt32(getElement("INT", ref element));
                item.CHA = Convert.ToInt32(getElement("CHA", ref element));

                //add new item to list
                items.Add(item);
            }
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public Item getItem(string name)
    {
        foreach (Item it in items)
        {
            if (it.Name == name) return it;
        }
        return null;
    }

}
}
