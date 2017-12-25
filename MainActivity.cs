using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
namespace wms
{
    [Activity(Label = "@string/app_name", LaunchMode = Android.Content.PM.LaunchMode.Multiple, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        Android.Support.V4.Widget.DrawerLayout _drawerLayout;
        ListView listview_leftMenu;
        Android.Support.V7.Widget.Toolbar toolbar;
        ActionBarDrawerToggle _drawerToggle;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //Set your main view here
            SetContentView(Resource.Layout.main);
            ProgressBar bar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            Button b_confirm = FindViewById<Button>(Resource.Id.button1);
            Button b_exit = FindViewById<Button>(Resource.Id.button2);
            b_confirm.Click += B_confirm_Click;
            b_exit.Click += (Object sender, EventArgs e) =>
            {
                //对话框
                var exitDialog = new Android.Support.V7.App.AlertDialog.Builder(this);
                //对话框内容
                exitDialog.SetMessage("Are you sure exit?");
                //拨打按钮
                exitDialog.SetNeutralButton("Yes", delegate
                {
                    Finish();
                });
                //取消按钮
                exitDialog.SetNegativeButton("Cancel", delegate { });
                //显示对话框
                exitDialog.Show();
            };
        }

        private void B_confirm_Click(object sender, EventArgs e)
        {
            ProgressBar bar = FindViewById<ProgressBar>(Resource.Id.progressBar1);
            EditText user_id = FindViewById<EditText>(Resource.Id.editText1);
            EditText user_password = FindViewById<EditText>(Resource.Id.editText2);
            WebApi api = new WebApi();
            if (user_id.Text.Trim() == "")
            {
                Toast.MakeText(this, "You must entry a valid username", ToastLength.Short).Show();
                user_id.RequestFocus();
                return;
            }
            if (user_password.Text.Trim() == "")
            {
                Toast.MakeText(this, "You must entry a valid password", ToastLength.Short).Show();
                user_password.RequestFocus();
                return;
            }
            if (api.Login(user_id.Text.Trim(), user_password.Text.Trim()))
            {
                SetContentView(Resource.Layout.menu);
                
                _drawerLayout = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.dl_left);
                listview_leftMenu = FindViewById<ListView>(Resource.Id.left_menu);
                toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
                _drawerToggle = new ActionBarDrawerToggle(this, _drawerLayout, toolbar, 0, 0);//并没有起效果性作用
                string[] menus = new string[] { "首页", "博问", "闪存" };
                ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleExpandableListItem1, menus);
                listview_leftMenu.Adapter = adapter;

                toolbar.Title = "Toolbar1";
                SetSupportActionBar(toolbar);//设置兼容toolbar，替代原本的actionbar

                //SupportActionBar.SetDisplayShowHomeEnabled(true);//设置显示左上角Home图标
                //SupportActionBar.SetDisplayHomeAsUpEnabled(true);//设置左上角的左箭头; 这两个必须同时为true才能显示
                SupportActionBar.SetDisplayShowTitleEnabled(true);//设置不显示标题
                SupportActionBar.SetHomeButtonEnabled(true);//设置返回键可用 
                SupportActionBar.Title = "Toolbar";
                toolbar.Title = "Toolbar1";
                toolbar.SetTitleTextColor(Resources.GetColor(Resource.Color.white));

                _drawerLayout.SetDrawerListener(_drawerToggle); //设置侧滑监听
                _drawerToggle.SyncState(); //设置左箭头与Home图标的切换与侧滑同步
                StatusBarUtil.SetColorStatusBar(this);
            }
            else
            {
                bar.Visibility = ViewStates.Invisible;
                Toast.MakeText(this, "user name or password is not valid", ToastLength.Short).Show();
            }
        }
    //图标动画效切换的关键：响应action 按钮的点击事件，包括左侧系统的home按钮，left按钮，右侧自定义的菜单等
    public override bool OnOptionsItemSelected(IMenuItem item)
        {
            return base.OnOptionsItemSelected(item) || _drawerToggle.OnOptionsItemSelected(item);
            //_drawerToggle.OnOptionsItemSelected(item)兼容android5.0以下的左侧图标切换的动画,不加这句android5.0以下的左上侧按钮无效
        }
    }
}







