using UdpQuickShare.ViewModels;

namespace UdpQuickShare.Pages;

public partial class FileItemView : Grid
{
    private FileItem fileItem;
    bool isSending => fileItem?.IsSendingFile ?? true;
    public FileItemView()
	{
		InitializeComponent();
	}
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        if(BindingContext is FileItem fileItem)
        {
            if (this.fileItem != null)
            {
                this.fileItem.PropertyChanged -= FileItem_PropertyChanged;
            }
            this.fileItem=fileItem;
            UpdateLayout(fileItem.State);
            fileItem.PropertyChanged += FileItem_PropertyChanged;
        }
        else
        {
            if(this.fileItem != null)
            {
                this.fileItem.PropertyChanged -= FileItem_PropertyChanged;
            }
        }
    }

    private void FileItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(FileItem.State))
        {
            UpdateLayout(fileItem.State);
        }
        else if (e.PropertyName == nameof(FileItem.Description))
        {
            if(fileItem.Description!= null)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Comment.Text = fileItem.Description;
                    Copy.IsVisible = true;
                    StopOrResume.IsVisible = false;
                    Open.IsVisible= false;
                });
            }
        }
        else if (e.PropertyName == nameof(FileItem.Working))
        {
            MainThread.BeginInvokeOnMainThread(() =>StopOrResume.Text=fileItem.Working?"��ͣ":"����");

        }
    }
    void UpdateLayout(FileItemState state)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            switch (state)
            {
                default:
                case FileItemState.Waiting:
                    WaitingLayout();
                    break;
                case FileItemState.Working:
                    WorkingLayout();
                    break;
                case FileItemState.Stopped:
                    StoppLayout();
                    break;
                case FileItemState.Completed:
                    CompletedLayout();
                    break;
            }
        });
    }
    void WaitingLayout()
    {
        Comment.Text = "�ȴ���";
        Open.IsVisible = false;
        Copy.IsVisible = false;
    }
    void WorkingLayout()
    {
        Comment.Text = "������";
        StopOrResume.Text = "��ͣ";
        StopOrResume.IsVisible = true;
        Open.IsVisible = false;
        Copy.IsVisible = false;

    }
    void StoppLayout()
    {
        Comment.Text = "����ͣ";
        StopOrResume.Text = "����";     
        StopOrResume.IsVisible= true;
        Open.IsVisible = false;
        Copy.IsVisible = false;
    }
    void CompletedLayout()
    {
        StopOrResume.IsVisible = false;
        Comment.Text =fileItem.Description??"�����";
        if (fileItem.Description != null)
        {
            Open.IsVisible = false;
            Copy.IsVisible = true;
        }
        else
        {
            Open.IsVisible = !isSending;
            Copy.IsVisible = false;
        }
    }
}