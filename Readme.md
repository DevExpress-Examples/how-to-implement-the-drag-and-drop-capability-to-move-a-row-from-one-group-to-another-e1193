# How to implement the drag-and-drop capability to move a row from one group to another


<p>The following example demonstrates how to implement the drag-and-drop capability, to enable end-users to drag any grid row from one group and drop it to another group, thus changing it.</p><p>To accomplish this task, it is necessary to set the <strong>GridView.AllowDrop</strong> property to <strong>True</strong>, and handle its <strong>PreviewMouseDown</strong>, <strong>PreviewMouseMove</strong>, <strong>DragOver</strong> and <strong>Drop</strong> events, as shown in this example.</p>

<br/>


