using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using DevExpress.Data;

namespace DragAndDrop_ChangeGroup {

    public partial class Window1 : Window {
        BindingList<TestData> list;
        bool dragStarted;

        public Window1() {
            InitializeComponent();

            #region Data binding
            list = new BindingList<TestData>();
            for (int i = 0; i < 100; i++) {
                list.Add(new TestData() {
                    Text = "row " + i,
                    Group = "group " + i / 5,
                    Number = i,
                });
            }

            grid.DataSource = list;
            grid.ExpandAllGroups();
            #endregion

            view.AllowDrop = true;

            view.PreviewMouseDown += new MouseButtonEventHandler(View_PreviewMouseDown);
            view.PreviewMouseMove += new MouseEventHandler(View_PreviewMouseMove);
            view.DragOver += new DragEventHandler(View_DragOver);
            view.Drop += new DragEventHandler(View_Drop);

        }

        void View_Drop(object sender, DragEventArgs e) {
            int rowHandle = (int)e.Data.GetData(typeof(int));
            TestData obj = (TestData)grid.GetRow(rowHandle);
            int dropRowHandle = view.GetRowHandleByTreeElement(e.OriginalSource as DependencyObject);
            if (grid.GroupCount == 1) {
                string fieldName = grid.SortInfo[0].FieldName;
                object groupValue = grid.GetCellValue(dropRowHandle, fieldName);
                if (IsCopyEffect(e)) {
                    TestData newData = 
                        new TestData() { Text = obj.Text + " (Copy)", 
                                         Number = obj.Number, 
                                         Group = obj.Group };
                    TypeDescriptor.GetProperties(typeof(TestData))[fieldName].SetValue(newData, groupValue);
                    list.Add(newData);
                }
                else {
                    TypeDescriptor.GetProperties(typeof(TestData))[fieldName].SetValue(obj, groupValue);
                }
            }
        }

        void View_DragOver(object sender, DragEventArgs e) {
            if (view.GetRowElementByTreeElement(e.OriginalSource as DependencyObject) != null 
            && grid.GroupCount == 1)
                e.Effects = IsCopyEffect(e) ? DragDropEffects.Copy : DragDropEffects.Move;
            else
                e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        bool IsCopyEffect(DragEventArgs e) {
            return (e.KeyStates & DragDropKeyStates.ControlKey) == DragDropKeyStates.ControlKey;
        }

        void View_PreviewMouseMove(object sender, MouseEventArgs e) {
            int rowHandle = view.GetRowHandleByMouseEventArgs(e);
            if (dragStarted) {
                DataObject data = CreateDataObject(rowHandle);
                DragDrop.DoDragDrop(view.GetRowElementByMouseEventArgs(e), data, 
                    DragDropEffects.Move | DragDropEffects.Copy);
                dragStarted = false;
            }
        }

        void View_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
            int rowHandle = view.GetRowHandleByMouseEventArgs(e);
            if (rowHandle != GridDataController.InvalidRow && !grid.IsGroupRowHandle(rowHandle))
                dragStarted = true;
        }

        private DataObject CreateDataObject(int rowHandle) {
            DataObject data = new DataObject();
            data.SetData(typeof(int), rowHandle);
            return data;
        }
    }

    #region TestData class
    public class TestData : INotifyPropertyChanged {
        string text;
        int number;
        string group;

        public string Text {
            get { return text; }
            set {
                if (Text == value)
                    return;
                text = value;
                OnPorpertyChanged("Text");
            }
        }
        public int Number {
            get { return number; }
            set {
                if (Number == value)
                    return;
                number = value;
                OnPorpertyChanged("Number");
            }

        }
        public string Group {
            get { return group; }
            set {
                if (Group == value)
                    return;
                group = value;
                OnPorpertyChanged("Group");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPorpertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    #endregion
}