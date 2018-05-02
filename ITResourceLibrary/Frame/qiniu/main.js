
var res = { "uptoken": "s_ryWUmImAw8dBId5EpJqMFhOt5hfmftDLX8Thm-:q4dvvre_IIJ_OTyko7zMotxoGmo=:eyJzY29wZSI6InBpY3R1cmUtZmlsZSIsImRlYWRsaW5lIjoxNTIzNjE1NzgyIH0=", "domain": "http://p0mrev3g3.bkt.clouddn.com/" }
    var token = res.uptoken;
    var domain = res.domain;
    var config = {
      useCdnDomain: true,
      disableStatisticsReport: false,
      retryCount: 6,
      region: qiniu.region.z0
    };
    var putExtra = {
      fname: "",
      params: {},
      mimeType: null
};
    $(".nav-box")
      .find("a")
      .each(function(index) {
        $(this).on("click", function(e) {
          switch (e.target.name) {
            case "h5":
              uploadWithSDK(token, putExtra, config, domain);
              break;
            case "expand":
              uploadWithOthers(token, putExtra, config, domain);
              break;
            case "directForm":
              uploadWithForm(token, putExtra, config);
              break;
            default:
              "";
          }
        });
      });
imageControl(domain);
//获取上传凭证
    $.ajax({
            type: "get",
            url: "getToken?type=1",
            success(o) {
                token = o;
                uploadWithSDK(token, putExtra, config, domain);
            }
        

});
$("#rename").click(function () {
    if ($("#newname").val().length > 0 && $("#oldname").val().length > 0 && $("#newname").val().indexOf("-") !== -1) {
        $("#renamesucces").attr("hidden", 'hidden');
        $.ajax({
            type: "get",
            url: "/FileManage/reName?oldname=" + $("#oldname").val() + "&newname=" + $("#newname").val(),
            success: function (result) {
                if (result.indexOf("error") !== -1) {
                    alert(result);
                } else {
                    $("#renamesucces").removeAttr("hidden");
                }
            }
        });
    } else {
        alert("文件名格式有误！");
    }
    
});
$("#deletefile").click(function () {
    if ($("#oldfilename").val().length > 0) {
        $("#deletesucces").attr("hidden",'hidden');
        $.ajax({
            type: "get",
            url: "/FileManage/delFile?filename=" + $("#oldfilename").val(),
            success: function (result) {
                if (result!=="success") {
                    alert(result);
                } else {
                    $("#deletesucces").removeAttr("hidden"); 
                }
            }
        });
    }
});

   

