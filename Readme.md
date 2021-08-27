<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128651482/13.1.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E1193)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [Window1.xaml](./CS/Window1.xaml) (VB: [Window1.xaml](./VB/Window1.xaml))
* [Window1.xaml.cs](./CS/Window1.xaml.cs) (VB: [Window1.xaml.vb](./VB/Window1.xaml.vb))
<!-- default file list end -->
# How to implement the drag-and-drop capability to move a row from one group to another


<p>The following example demonstrates how to implement the drag-and-drop capability, to enable end-users to drag any grid row from one group and drop it to another group, thus changing it.</p><p>To accomplish this task, it is necessary to set the <strong>GridView.AllowDrop</strong> property to <strong>True</strong>, and handle its <strong>PreviewMouseDown</strong>, <strong>PreviewMouseMove</strong>, <strong>DragOver</strong> and <strong>Drop</strong> events, as shown in this example.</p>

<br/>


