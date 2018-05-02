function uploadWithSDK(token, putExtra, config, domain) {
    var url = "http://p0mrev3g3.bkt.clouddn.com/";
    var urll = "http://p0y9ixilz.bkt.clouddn.com/";//files空间
    // 切换tab后进行一些css操作
    controlTabDisplay("sdk");
    $("#select2").unbind("change").bind("change", function () {
        var file = this.files[0];
        var finishedAttr = [];
        // eslint-disable-next-line
        var compareChunks = [];
        var observable;
        if (file) {

           
            var end = file.name.lastIndexOf(".");
            var username;
            $.ajax({
                type: "get",
                url: "/Shared/GetUserName",
                async: false,
                success: function (name) {
                    username = name;
                }
            });
            var key;
            var lastname="-" + username + file.name.substring(end, file.name.length)
            if (file.name.lastIndexOf("-") !== -1) {
                key = file.name.substring(0, file.name.lastIndexOf("-")) + lastname;
            } else {
                key = file.name.substring(0, end) + lastname;
            }
            
            
            //============判断文件类型====================
            if (key && key.match(/\.(jpg|jpeg|png|gif|JPG|JPEG|PNG|GIF)$/)) {
                //判断文件是否存在
                $.ajax({
                    url: url + key + "?v=" + Date.parse(new Date()),
                    type: "get",
                    async: false,
                    success(result) {
                        if (result.hasOwnProperty('error')) {
                            token=getFileToken(token,0);
                        } else {
                             alert("空间存在此图片！继续操作则会覆盖。");
                           token=getkeyToken(token,0,key);
                        }
                    }
                    , error(e) {
                        token=getFileToken(token,0);
                    }

                });
            } else {
                //判断文件是否存在
                $.ajax({
                    url: urll + key+"?attname=&"+ "v=" + Date.parse(new Date()),
                    type: "get",
                    async: false,
                    success(result) {
                        if (result.hasOwnProperty('error')) {
                            token=getFileToken(token,1);
                        } else {
                            alert("空间存在此文件！继续操作则会覆盖。");
                            token=getkeyToken(token,1,key);
                           
                        }
                    },
                    error(e) {
                          token=getFileToken(token,1);
                    }
                });
            }
            // 添加上传dom面板
            var board = addUploadBoard(file, config, key, "");
            if (!board) {
                return;
            }
            putExtra.params["x:name"] = key.split(".")[0];
            board.start = true;
            var dom_total = $(board)
                .find("#totalBar")
                .children("#totalBarColor");

            // 设置next,error,complete对应的操作，分别处理相应的进度信息，错误信息，以及完成后的操作
            var error = function (err) {
                board.start = true;
                $(board).find(".control-upload").text("继续上传");
                console.log(err);
                alert(err.message)
            };

            var complete = function (res) {
                $(board)
                    .find("#totalBar")
                    .addClass("hide");
                $(board)
                    .find(".control-container")
                    .html(
                    "<p><strong>Hash：</strong>" +
                    res.hash +
                    "</p>" +
                    "<p><strong>Bucket：</strong>" +
                    res.bucket +
                    "</p>"
                    );
                if (res.key && res.key.match(/\.(jpg|jpeg|png|gif)$/)) {

                    imageDeal(board, res.key, domain);
                    $(board).find("#urlpath").html(url + res.key+ "?v=" + Date.parse(new Date()));
                } else {
                    $(board).find("#urlpath").html(urll + res.key+"?attname="+ "v=" + Date.parse(new Date()));
                }
                
            };
            var next = function (response) {
                var chunks = response.chunks || [];
                var total = response.total;
                // 这里对每个chunk更新进度，并记录已经更新好的避免重复更新，同时对未开始更新的跳过
                for (var i = 0; i < chunks.length; i++) {
                    if (chunks[i].percent === 0 || finishedAttr[i]) {
                        continue;
                    }
                    if (compareChunks[i].percent === chunks[i].percent) {
                        continue;
                    }
                    if (chunks[i].percent === 100) {
                        finishedAttr[i] = true;
                    }
                    $(board)
                        .find(".fragment-group li")
                        .eq(i)
                        .find("#childBarColor")
                        .css(
                        "width",
                        chunks[i].percent + "%"
                        );
                }
                $(board)
                    .find(".speed")
                    .text("进度：" + total.percent + "% ");


                dom_total.css(
                    "width",
                    total.percent + "%"
                );
                compareChunks = chunks;
            };

            var subObject = {
                next: next,
                error: error,
                complete: complete
            };
            var subscription;
            // 调用sdk上传接口获得相应的observable，控制上传和暂停
            observable = qiniu.upload(file, key, token, putExtra, config);
            //上传文件
            $(board)
                .find(".control-upload")
                .on("click", function () {
                    if (board.start) {
                        $(this).text("暂停上传");
                        board.start = false;
                        subscription = observable.subscribe(subObject);
                    } else {
                        board.start = true;
                        $(this).text("继续上传");
                        subscription.unsubscribe();
                    }
                });
            $(board).find(".copypath")
                .on("click", function () {
                    var Url2 = $(board).find("#urlpath");
                    Url2.select(); // 选择对象

                    document.execCommand("Copy"); // 执行浏览器复制命令
                    alert("已复制好，可贴粘。");
                })

        }
    })
}
function getFileToken(token,type) {
       $.ajax({
            url: "getToken?type="+type,
            type: "get",
            async: false,
            success: function (str) {
                token = str;
            },
            error: function (e) {
                alert("Token请求出错");
            }
    });
     return token;
}
function getkeyToken(token,type,key) {
       $.ajax({
            url: "getTokenkey?type="+type+"&key="+key,
            type: "get",
            async: false,
            success: function (str) {
                token=str;
            },
            error: function (e) {
                alert("Token请求出错");
            }
    });
    return token;
    
}