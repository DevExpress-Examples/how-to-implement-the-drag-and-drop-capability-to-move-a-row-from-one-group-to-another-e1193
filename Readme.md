<!-- default file list -->
*Files to look at*:

* [Window1.xaml](./CS/Window1.xaml) (VB: [Window1.xaml](./VB/Window1.xaml))
* [Window1.xaml.cs](./CS/Window1.xaml.cs) (VB: [Window1.xaml.vb](./VB/Window1.xaml.vb))
<!-- default file list end -->
# How to implement the drag-and-drop capability to move a row from one group to another


<p>The following example demonstrates how to implement the drag-and-drop capability, to enable end-users to drag any grid row from one group and drop it to another group, thus changing it.</p><p>To accomplish this task, it is necessary to set the <strong>GridView.AllowDrop</strong> property to <strong>True</strong>, and handle its <strong>PreviewMouseDown</strong>, <strong>PreviewMouseMove</strong>, <strong>DragOver</strong> and <strong>Drop</strong> events, as shown in this example.</p>

<br/>


