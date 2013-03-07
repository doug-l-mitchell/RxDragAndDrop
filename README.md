RxDragAndDrop
=============

Add Drag And Drop capabilities to a xaml view by subscribing to a few observers.

Observables
-----------

- ObserveMouseLeftButtonDown
- ObserveMouseLeftButtonUp
- ObserveMouseMove
- ObserveMouseLeave
- ObserveBeginMouseDrag
- ObservePreviewDragEnter
- ObservePreviewDragOver
- ObservePreviewDrop
- ObservePreivewDragLeave

Usage
-----
```csharp
    public partial class MyView : UserControl
    {
    	InitializeComponent();

    	this.ObserveBeginMouseDrag()
    		.Subscribe(e => 
    		{
				// collect the data to drag...
				// var data = ...

				var dragData = new DataObject("myDragData", data);
				DragDrop.DoDragDrop(this, dragData, DragDropEffects.Move);
    		});

    	this.ObservePreviewDragOver()
    		.Subscribe(e => 
    		{
    			var canDrop = e.EventArgs.Data.GetDataPresent("myDragData");
    			e.EventArgs.Effects = canDrop ? DragDropEffects.Move : DragDropEffects.None;
    			e.EventArgs.Handled = canDrop;

    			if(canDrop)
    				ShowDraggedAdorner(e.Sender, e.EventArgs);
			});

		this.ObservePreviewDrop()
			.Subscribe(e => 
			{
				RemoveDraggedAdorner();
				var dragData = e.EventArgs.Data.GetData("myDragData") as ...;

				if(dragData != null)
				{
					// do something with the data
				}
			});
    }
```