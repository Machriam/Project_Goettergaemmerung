﻿using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Project_Goettergaemmerung.Components.CardInformationGetter;
using Project_Goettergaemmerung.Components.Model;
using Project_Goettergaemmerung.ExtensionMethods;

namespace Project_Goettergaemmerung.Components
{
    public interface ISaveImage
    {
        void SaveCardasImage(Bitmap card, string filename, CardType type, int print1, int print2, int print3, int print4);
    }

    public class SaveImage : ISaveImage
    {
        private readonly IUserData _userData;

        public SaveImage(IUserData userData)
        {
            _userData = userData;
        }

        private string ChangePrintNumbers(int print)
        {
            return string.Format("{0:000}", print);
        }

        private void SaveNormalFormat(Bitmap card, string filename, CardType type, int print)
        {
            var name = type.GetDescription() + "_" + ChangePrintNumbers(print) + "_" + filename;
            card.Save(_userData.ExportPath + "\\" + name + ".png", ImageFormat.Png);
        }

        private void SaveRebalenceFormat(Bitmap card, string filename, CardType type, int print)
        {
            var name = _userData.RebalenceNumber;
            card.Save(_userData.ExportPath + "\\" + name + ".png", ImageFormat.Png);
        }

        private void SaveTabeltopFormat(Bitmap card, string filename, CardType type, int print)
        {
            for (int i = 0; i < print; i++)
            {
                var name = type.GetDescription() + "_" + ChangePrintNumbers(i + 1) + "_" + filename;
                card.Save(_userData.ExportPath + "\\" + name + ".png", ImageFormat.Png);
            }
        }

        public void SaveCardasImage(Bitmap card, string filename, CardType type,
            int print1, int print2, int print3, int print4)
        {
            int print;
            _userData.RebalenceNumber += 1;

            switch (_userData.Printer)
            {
                case PrintType.Print1:
                    print = print1;
                    break;

                case PrintType.Print2:
                    print = print2;
                    break;

                case PrintType.Print3:
                    print = print3;
                    break;

                case PrintType.Print4:
                    print = print4;
                    break;

                default:
                    print = 0;
                    break;
            }

            if (print != 0)
            {
                if (_userData.CurrentFormat == SaveFormat.normal)
                {
                    SaveNormalFormat(card, filename, type, print);
                }
                if (_userData.CurrentFormat == SaveFormat.tabeltop)
                {
                    SaveTabeltopFormat(card, filename, type, print);
                }
                if (_userData.CurrentFormat == SaveFormat.rebalence)
                {
                    SaveRebalenceFormat(card, filename, type, print);
                }
            }
        }
    }
}
