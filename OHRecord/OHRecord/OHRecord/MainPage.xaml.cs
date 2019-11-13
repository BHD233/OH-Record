using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OHRecord
{
    public class RecordTitle
    {
        public static string backGroundColor = "333333";
        public Button summaryButton = new Button
        {
            Text = "...",
            BackgroundColor = Color.FromHex(backGroundColor),
            TextColor = Color.White,
            WidthRequest = Device.GetNamedSize(NamedSize.Small, typeof(Button)) * 3,
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
        };
        public Button viewButton = new Button
        {
            Text = "View",
            BackgroundColor = Color.FromHex(backGroundColor),
            TextColor = Color.White,
            WidthRequest = Device.GetNamedSize(NamedSize.Small, typeof(Button)) * 4,
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
        };
        public Button deleteButton = new Button
        {
            Text = "X",
            BackgroundColor = Color.FromHex(backGroundColor),
            TextColor = Color.White,
            WidthRequest = Device.GetNamedSize(NamedSize.Small, typeof(Button)) * 3,
            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
        };
        public Label name = new Label
        {
            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            BackgroundColor = Color.FromHex(backGroundColor),
            TextColor = Color.White,
        };
        public StackLayout GetTitle()
        {
            StackLayout mainStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            mainStack.Children.Add(deleteButton);
            mainStack.Children.Add(name);
            mainStack.Children.Add(summaryButton);
            mainStack.Children.Add(viewButton);

            return mainStack;
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public static string endFile = ".BHD";
        IFileHelper fileHelper = DependencyService.Get<IFileHelper>();

        public MainPage()
        {
            Title = "Main Page";

            InitializeComponent();

            //RefreshList();
        }

        private string GetFileName(string titleString)
        {
            string result = "";

            //add to title string
            for (int i = titleString.Length - 1; i > 0 && titleString[i] != '/' && titleString[i] != '\\'; i--)
            {
                result += titleString[i];
            }

            //revert it
            string temp = "";

            for (int i = result.Length - 1; i >= 0; i--)
            {
                temp += result[i];
            }

            return temp;
        }

        private string GetRidOfEndFile(string filename)
        {
            string result = "";

            for (int i = 0; i < filename.Length - endFile.Length; i++)
            {
                result += filename[i];
            }

            return result;
        }

        void RefreshList()
        {
            listStack.Children.Clear();
            IEnumerable<string> titleStrings = fileHelper.GetFiles();

            //ensure that it is all my file and get just file name
            List<string> filenames = new List<string>();
            foreach (string title in titleStrings)
            {
                string filename = GetFileName(title);
                if (IsMyFile(filename))
                {
                    filename = GetRidOfEndFile(filename);
                    filenames.Add(filename);
                }
            }

            //sort filename
            SortByDate(ref filenames);

            foreach (string filename in filenames)
            {
                RecordTitle currTitle = new RecordTitle();

                //create handle and ID for button
                currTitle.summaryButton.StyleId = filename;
                currTitle.viewButton.StyleId = filename;
                currTitle.deleteButton.StyleId = filename;
                currTitle.summaryButton.Clicked += OnSummaryButtonClick;
                currTitle.viewButton.Clicked += OnViewButtonClick;
                currTitle.deleteButton.Clicked += OnDeleteButtonClick;

                //get tile to lable
                currTitle.name.Text = filename;

                //add to stack
                listStack.Children.Add(currTitle.GetTitle());
            }
        }

        private bool IsMyFile(string filename)
        {
            int endFilePos = endFile.Length - 1;

            for (int i = filename.Length - 1; i > 0; i--)
            {
                //is match with end file
                if (endFilePos < 0)
                {
                    break;
                }
                else if (filename[i] != endFile[endFilePos])
                {
                    return false;
                }
                else
                {
                    endFilePos--;
                }
            }

            return true;
        }

        //Assume that date in format dd-mm
        private int GetRelativeDateOfString(string filename)
        {
            int date = 0;
            bool isMonth = false;
            int day = 0;
            int month = 0;

            for (int i = 0; i < filename.Length; i++)
            {
                if (filename[i] >= '0' && filename[i] <= '9')
                {
                    if (!isMonth)
                    {
                        day = day * 10 + int.Parse(filename[i].ToString());
                    }
                    else
                    {
                        month = month * 10 + int.Parse(filename[i].ToString());
                    }
                }
                if (filename[i] == '-')
                {
                    isMonth = true;
                }
            }
            date = month * 10 + date;

            return date;
        }

        private List<int> GetRelativeDateOfList(List<string> filenames)
        {
            List<int> dates = new List<int>();

            foreach (string filename in filenames)
            {
                dates.Add(GetRelativeDateOfString(filename));
            }

            return dates;
        }

        private void Swap<T>(ref List<T> list, int a, int b)
        {
            T temp;
            temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }

        private void SortByDate(ref List<string> filenames)
        {
            //get ralative date to sort
            List<int> dates = GetRelativeDateOfList(filenames);

            //sort by relative date
            for (int i = 0; i < filenames.Count; i++)
            {
                for (int j = i + 1; j < filenames.Count; j++)
                {
                    if (dates[i] < dates[j])
                    {
                        Swap<int>(ref dates, i, j);
                        Swap<string>(ref filenames, i, j);
                    }
                }
            }
        }

        async void OnAddButtonClick(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new NewRecordPage());
        }

        async void OnSummaryButtonClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new SummaryPage(button.StyleId));
        }

        async void OnViewButtonClick(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            await Navigation.PushAsync(new OpenRecordPage(button.StyleId));
        }

        async void OnDeleteButtonClick(object sender, EventArgs args)
        {
            bool isDeleteComfirmed = false;
            Button button = (Button)sender;

            Task<bool> comfirmDelete = DisplayAlert("Are you sure to delete the select item", "", "yes", "no");
            isDeleteComfirmed = await comfirmDelete;

            if (true == isDeleteComfirmed)
            {
                string filename = button.StyleId;
                fileHelper.Delete(filename);
                RefreshList();
            }
        }
        
        void OnRefreshButtonClick(object sender, EventArgs args)
        {
            RefreshList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            RefreshList();

        }
    }
}