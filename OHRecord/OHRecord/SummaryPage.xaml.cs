using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OHRecord
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SummaryPage : ContentPage
    {
        List<string> labelText = new List<string>();
        FileHelper fileHelper = new FileHelper();
        public SummaryPage(string filename)
        {
            InitializeComponent();
            string text = fileHelper.ReadText(filename);
            Proccess(text);
            foreach (string t in labelText)
            {
                Entry currEntry = new Entry
                {
                    Text = t,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                recordStack.Children.Add(currEntry);
            }
        }

        string GetRidOfTimeCount(string text)
        {
            string outStr = "";
            int timeStrLength = 0;
            bool metSpace = false;
            for (int i = text.Length - 1; i >= 0; i--)
            {
                if (text[i] == ' ')
                {
                    metSpace = true;
                }
                if (metSpace && text[i] != ' ')
                {
                    break;
                }
                timeStrLength++;
            }
            for (int i = 0; i < text.Length - timeStrLength; i++)
            {
                outStr += text[i];
            }
            return outStr;
        }

        void Proccess(string text)
        {
            string currtext = "";
            List<string> rawLabel = new List<string>();

            int i = 2;      //because 2 char is the index (1 )

            while (i < text.Length)
            {
                if(text[i] == '\n')
                {
                    currtext = GetRidOfTimeCount(currtext);
                    rawLabel.Add(currtext);
                    currtext = "";
                    //remove the first digit (index)
                    i += 2;
                }
                else
                {
                    currtext += text[i];
                }
                i++;
            }
            rawLabel.Sort();

            currtext = rawLabel[0];
            i = 0;
            int sum = 1;
            while(i < rawLabel.Count)
            {
                if(currtext != rawLabel[i])
                {
                    currtext = currtext + "  " + sum.ToString();
                    labelText.Add(currtext);
                    currtext = rawLabel[i];
                    sum = 1;
                }
                else
                {
                    sum++;
                }
                i++;
            }
        }
    }
}