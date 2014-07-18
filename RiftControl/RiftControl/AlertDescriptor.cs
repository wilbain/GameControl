using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace RiftControl
{
    public class AlertDescriptor
    {
        public string Name { get; set; }
        public AlertDescriptorType Type { get; set;}
        public Point AlertPosition { get; set; }
        public String AlertShownColour { get; set; }

        public AlertDescriptor()
        {
        }

        public AlertDescriptor(string name, AlertDescriptorType type, Point position, Color colour)
        {
            Name = name;
            Type = type;
            AlertPosition = position;
            AlertShownColour = colour.ToString();
        }

        static internal void SerializeToXml(AlertDescriptor descriptor, string filename)
        {
            SerializeToXml(new List<AlertDescriptor>() { descriptor }, filename);
        }

        static public void SerializeToXml(List<AlertDescriptor> descriptors, string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<AlertDescriptor>));
            TextWriter textWriter = new StreamWriter(filename);
            serializer.Serialize(textWriter, descriptors);
            textWriter.Close();
        }

        static public List<AlertDescriptor> DeserializeFromXml(string filename)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(List<AlertDescriptor>));
            TextReader textReader = new StreamReader(filename);
            List<AlertDescriptor> descriptors = (List<AlertDescriptor>)deserializer.Deserialize(textReader);
            textReader.Close();

            return descriptors;
        }
    }

    public enum AlertDescriptorType
    {
        Cooldown,
        Casting,
        Active,
        Available,
        Expiring,
        Missing
    }
}