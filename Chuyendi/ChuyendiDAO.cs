using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Chuyendi
{
    class ChuyendiDAO
    {
        public static string WorkingDirectory = System.IO.Directory.GetCurrentDirectory().Replace('\\', '/') + "/";
        public static string DataLink= WorkingDirectory + "data/data.xml";

        public static List<TTChuyendi> virtualDataBase;

        public static void ConnectToVirtualDataBase()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<TTChuyendi>));

            using (Stream reader = new FileStream(DataLink, FileMode.Open))
            {
                // Call the Deserialize method to restore the object's state.
                virtualDataBase = (List<TTChuyendi>)serializer.Deserialize(reader);
            }            
        }

        public static void SaveData(List<TTChuyendi> nhungchuyendi)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<TTChuyendi>));

            TextWriter writer = new StreamWriter(DataLink);
            ser.Serialize(writer, nhungchuyendi);
            writer.Close();

        }

        public static List<TTChuyendi> GetAll()
        {           
            return virtualDataBase;
        }


        // Use to generate a fake Data to test
        public static List<TTChuyendi> FakeData()
        {
            var nhungchuyendi = new List<TTChuyendi>();

            nhungchuyendi.Add( new TTChuyendi() { Name = "Đi xả stress", Place = "Cần thơ", Status = 0, ImgLink="imgs/splash.jpg",
                Members = new List<Thanhvien>() { 
                    new Thanhvien() { Name = "Nam", 
                        Bills = new List<Bill>() {
                            new Bill("Xăng", 100),
                            new Bill("Nước uống", 100),
                            new Bill("Seven up", 50),
                        }
                    },
                    new Thanhvien() { Name = "Quốc",
                        Bills = new List<Bill>() {
                            new Bill("Thuê chỗ ở", 900),
                        }
                    },
                    new Thanhvien() { Name = "Sơn",
                        Bills = new List<Bill>() {
                            new Bill("Than", 50),
                            new Bill("Bật lửa", 5),
                            new Bill("Dầu hỏa",120),
                            new Bill("Dụng cụ nấu ăn",230),
                        }
                    },
                    new Thanhvien() { Name = "Hà",
                        Bills = new List<Bill>() {
                            new Bill("Thịt heo", 135),
                            new Bill("Gia vị, hương liệu",60),
                            new Bill("Rau củ quả", 120),
                            new Bill("Cá thu", 90),
                            new Bill("Tôm hùm", 300),
                        }
                    },
                }                       
            }) ;

            nhungchuyendi.Add(new TTChuyendi()
            {
                Name = "Đi chơi tết",
                Place = "Phú Quốc",
                Status = 4,
                ImgLink = "imgs/splash.jpg",
                Members = new List<Thanhvien>() {
                    new Thanhvien() { Name = "Quyết",
                        Bills = new List<Bill>() {
                            new Bill("Vé tàu", 2500),
                            new Bill("Bia", 150),
                            
                        }
                    },
                    new Thanhvien() { Name = "Thắng",
                        Bills = new List<Bill>() {
                            new Bill("Thuê chỗ ở", 1200),
                        }
                    },
                    new Thanhvien() { Name = "Trường",
                        Bills = new List<Bill>() {                            
                            new Bill("Thuê xe",500),
                        }
                    },
                    new Thanhvien() { Name = "Hà",
                        Bills = new List<Bill>() {
                            new Bill("Ăn nhà hàng",1750),
                        }
                    },
                }
            });

            return nhungchuyendi;
        }
    }
}
