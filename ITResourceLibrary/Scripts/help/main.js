var allDialog = [];
closeDialog = function () {
    if (allDialog.length > 0) {
        allDialog[allDialog.length - 1].close();
        allDialog.remove(allDialog[allDialog.length - 1]);
    }
}

reloadDialog = function () {
    var childWindow = document.getElementsByTagName("iframe");
    if (childWindow.length > 0) {
        for (var i in childWindow) {
            if (childWindow[i].contentWindow) {
                console.log(childWindow[i])
                childWindow[i].contentWindow.location.reload();
            }
        }
    }
}
var maxWidth = 750;
var maxHeight = 450;

function openDialog(url, title, size) {
    var currentDialog = dialog({
        width: size && size.width ? size.width : maxWidth,
        height: size && size.height ? size.height : maxHeight,
        fixed: true,
        content: '<iframe src="' + url + '" frameborder="0" style="width:100%; height:100%;"></iframe>',
        title: title,
    }).showModal();
    allDialog.push(currentDialog);
    return currentDialog;
}