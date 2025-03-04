using System.Collections.ObjectModel;
using UdpQuickShare.ViewModels;

namespace UdpQuickShare.Pages;

public partial class SaveFolderSetter : ContentPage
{
	public SaveFolderSetter()
	{
		InitializeComponent();
        Folders.ItemsSource = CreateFolders();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UseDirectPathSwitch.IsToggled = FileManager.CanUseDirectPath;
    }
    ObservableCollection<FolderItem> CreateFolders()
	{
        var collection = new ObservableCollection<FolderItem>
        {
            new FolderItem
            {
                FileType = FileActions.FileType.Text,
                Name = "文本",
                Path =FileSaveManager.GetSaveFolder(FileActions.FileType.Text),
            },
            new FolderItem
            {
                FileType = FileActions.FileType.Image,
                Name = "图片",
                Path = FileSaveManager.GetSaveFolder(FileActions.FileType.Image),
            },
            new FolderItem
            {
                FileType = FileActions.FileType.Audio,
                Name = "音频",
                Path = FileSaveManager.GetSaveFolder(FileActions.FileType.Audio),
            },
            new FolderItem
            {
                FileType = FileActions.FileType.Video,
                Name = "视频",
                Path = FileSaveManager.GetSaveFolder(FileActions.FileType.Video),
            },
            new FolderItem
            {
                FileType = FileActions.FileType.Any,
                Name = "其他",
                Path = FileSaveManager.GetSaveFolder(FileActions.FileType.Any),
            }
        };
        return collection;
    }

    private async void Folders_ItemTapped(object sender, ItemTappedEventArgs e)
    {
		var foler = e.Item as FolderItem;
		await FileSaveManager.ChooseSaveFolder(foler.FileType);
		foler.Path=FileSaveManager.GetSaveFolder(foler.FileType); 
    }

    private async void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        FileManager.UseDirectPath = e.Value;
        App.DataStore.Save("UseDirectPath", e.Value);
        if (e.Value)
        {
            var success = await FileManager.RequsetUseDirectPathNeed();
            if (!success)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    UseDirectPathSwitch.IsToggled = false;
                });
            }
        }
    }
}