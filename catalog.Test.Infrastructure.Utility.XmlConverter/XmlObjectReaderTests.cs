using catalog.Infrastructure.Utility.XmlConverter;
using catalog.Test.Infrastructure.Utility.XmlConverter.Setup;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace catalog.Test.Infrastructure.Utility.XmlConverter;

public class XmlObjectReaderTests
{
    class PacketHeader : IEquatable<PacketHeader>
    {
        public decimal Version { get; set; }
        public DateTime UpdateDate { get; set; }

        public bool Equals([AllowNull] PacketHeader other)
        {
            if (other == null || GetType() != other.GetType())
                return false;

            return (Version, UpdateDate) == (other.Version, other.UpdateDate);
        }
    }

    class PacketItemValue : IEquatable<PacketItemValue>
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public bool Equals([AllowNull] PacketItemValue other)
        {
            if (other == null || GetType() != other.GetType())
                return false;

            return (Code, Name) == (other.Code, other.Name);
        }
    }

    class PacketItem : IEquatable<PacketItem>
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
        public PacketItemValue Value { get; set; }

        public bool Equals([AllowNull] PacketItem other)
        {
            if (other == null || GetType() != other.GetType())
                return false;

            return
                (Value == null && other.Value == null || Value.Equals(other.Value)) &&
                (Code, Name, DateBegin, DateEnd) == (other.Code, other.Name, other.DateBegin, other.DateEnd);
        }
    }

    class Packet : IEquatable<Packet>
    {
        public PacketHeader Header { get; set; }
        public PacketItem Item { get; set; }

        public bool Equals([AllowNull] Packet other)
        {
            if (other == null || GetType() != other.GetType())
                return false;

            return Header.Equals(other.Header) && Item.Equals(other.Item);
        }
    }

    class MoDistrict : IEquatable<MoDistrict>
    {
        public string CodeMo { get; set; }
        public int Unit { get; set; }
        public int District { get; set; }
        public string Name { get; set; }
        public int Category { get; set; }
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }

        public bool Equals([AllowNull] MoDistrict other)
        {
            if (other == null || GetType() != other.GetType())
                return false;

            return
                (CodeMo, Unit, District, Name, Category, DateBegin, DateEnd) ==
                (other.CodeMo, other.Unit, other.District, other.Name, other.Category, other.DateBegin, other.DateEnd);
        }
    }

    [Fact]
    public void TestObjectValue()
    {
        using var flatXmlClass = XmlSetup.GenerateXmlClass();
        using var xmlObjectReaderClass = new XmlObjectReader(XmlSetup.GetXmlClassSchema(), flatXmlClass);

        var result = xmlObjectReaderClass.ReadObject("packet.zglv", out PacketHeader packetHeader);
        Assert.True(result);
        Assert.NotNull(packetHeader);
        Assert.Equal(new PacketHeader()
        {
            Version = 1.1m,
            UpdateDate = DateTime.Parse("03.10.2019")
        }, packetHeader);

        result = xmlObjectReaderClass.ReadObject("packet.entry", out PacketItem packetItem);
        Assert.True(result);
        Assert.NotNull(packetItem);
        Assert.Equal(new PacketItem()
        {
            Code = 0,
            Name = "Не требуется изготовление временного свидетельства",
            DateBegin = DateTime.Parse("01.10.2019"),
        }, packetItem);

        result = xmlObjectReaderClass.ReadCollection("packet.entry", out IEnumerable<PacketItem> packetItemCollection);
        Assert.True(result);
        Assert.NotNull(packetItemCollection);
        Assert.Equal(2, packetItemCollection.Count());

        Assert.Equal(new PacketItem()
        {
            Code = 1,
            Name = "Временное свидетельство на бумажном бланке",
            DateBegin = DateTime.Parse("01.05.2011"),
        }, packetItemCollection.First());

        Assert.Equal(new PacketItem()
        {
            Code = 2,
            Name = "Временное свидетельство в электронной форме",
            DateBegin = DateTime.Parse("01.06.2019"),
        }, packetItemCollection.Skip(1).First());

        using var flatXmlCollection = XmlSetup.GenerateXmlCollection();
        using var xmlObjectReaderCollection = new XmlObjectReader(XmlSetup.GetXmlCollectionSchema(), flatXmlCollection);

        result = xmlObjectReaderCollection.ReadCollection("root.rec", out IEnumerable<MoDistrict> districtCollection);
        Assert.True(result);
        Assert.NotNull(districtCollection);
        Assert.Equal(5, districtCollection.Count());

        Assert.Equal(new MoDistrict()
        {
            CodeMo = "750004",
            Unit = 1,
            District = 1,
            Category = 1,
            Name = "терапевтический участок №1",
            DateBegin = DateTime.Parse("01.01.2017")
        }, districtCollection.First());

        Assert.Equal(new MoDistrict()
        {
            CodeMo = "750004",
            Unit = 1,
            District = 2,
            Category = 1,
            Name = "терапевтический участок №2",
            DateBegin = DateTime.Parse("01.01.2017"),
            DateEnd = DateTime.Parse("01.01.2019")
        }, districtCollection.Skip(1).First());

        Assert.Equal(new MoDistrict()
        {
            CodeMo = "750004",
            Unit = 1,
            District = 3,
            Category = 1,
            Name = "терапевтический участок №3",
            DateBegin = DateTime.Parse("01.01.2017")
        }, districtCollection.Skip(2).First());
        Assert.Equal(new MoDistrict()
        {
            CodeMo = "750004",
            Unit = 1,
            District = 4,
            Category = 2,
            Name = "педиатрический участок №1",
            DateBegin = DateTime.Parse("01.01.2017")
        }, districtCollection.Skip(3).First());
        Assert.Equal(new MoDistrict()
        {
            CodeMo = "750004",
            Unit = 1,
            District = 5,
            Category = 2,
            Name = "педиатрический участок №2",
            DateBegin = DateTime.Parse("01.01.2017")
        }, districtCollection.Skip(4).First());

        using var flatXmlComplexClass = XmlSetup.GenerateXmlComplexClass();
        using var xmlObjectReaderComplexClass = new XmlObjectReader(XmlSetup.GetXmlComplexClassSchema(), flatXmlComplexClass);

        result = xmlObjectReaderComplexClass.ReadCollection("packets.packet", out IEnumerable<Packet> packetCollection);
        Assert.True(result);
        Assert.NotNull(packetCollection);
        Assert.Equal(2, packetCollection.Count());
        Assert.Equal(new Packet()
        {
            Header = new PacketHeader()
            {
                Version = 1.1m,
                UpdateDate = DateTime.Parse("01.10.2019")
            },
            Item = new PacketItem()
            {
                Value = new PacketItemValue()
                {
                    Code = "000",
                    Name = "Значение 000"
                },
                Code = 0,
                Name = "Не требуется изготовление временного свидетельства",
                DateBegin = DateTime.Parse("01.10.2019"),
            }
        }, packetCollection.First());
        Assert.Equal(new Packet()
        {
            Header = new PacketHeader()
            {
                Version = 1.2m,
                UpdateDate = DateTime.Parse("01.10.2020")
            },
            Item = new PacketItem()
            {
                Value = new PacketItemValue()
                {
                    Code = "001",
                    Name = "Значение 001"
                },
                Code = 1,
                Name = "Временное свидетельство на бумажном бланке",
                DateBegin = DateTime.Parse("01.10.2020"),
            }
        }, packetCollection.Skip(1).First());
    }
}
