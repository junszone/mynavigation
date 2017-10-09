var App = function () {
    var currentPage = '';
    $.jun = new Object();
    //Server列表
    var handleServerIndex = function (param) {
        //列表搜索条件
        $.jun.Server_Searchcondition = function () {
            var form = $('#tool');
            var data = {
                level: form.find('[name="level"]').val(),
                logic: form.find('input[name="logic"]').is(':checked') ? "or" : "and",//包含任意关键字
                keyword: form.find('[name="keyword"]').val()
            };
            return data;
        };
        $.jun.Server_QueryParams = function (params) {
            var data = $.jun.Server_Searchcondition();
            data.limit = params.limit;
            data.offset = params.offset;
            data.sort = params.sort;
            data.sortOrder = params.sortOrder;
            data.search = params.search;
            return data;
        };
        $.jun.Server_detail = function (id) {
            layer.open({
                title: '新增导航',
                type: 2,
                area: ['1200px', '660px'],
                fixed: true,
                maxmin: true,
                content: '/server/edit/' + id + ''
            });
        };
        $.jun.ServerTableInit = function () {
            $.jun.spinner.spin($('body').get(0));
            $("#serverTable").bootstrapTable({
                method: 'post',
                url: "/ServerData/RetrievePaged",
                idField: 'ID',
                striped: true,//true隔行变色
                pagination: true,//true表格底部显示分页条
                pageList: [10, 20, 30, 40, 50],
                search: false,//true启用搜索框
                cache: false,//false使用缓存
                sidePagination: "server", //服务端请求
                queryParams: $.jun.Server_QueryParams,
                pageSize: 20,//每页条数
                pageNumber: 1,//初始页码
                clickToSelect: true,
                singleSelect: true,
                showHeader: true,//true显示列头
                showFooter: false,//true显示列脚
                showColumns: false,//是否显示内容下拉框
                showRefresh: false,//是否显示刷新按钮
                showToggle: false,//是否显示切换视图按钮
                checkbox: false,//True to show a checkbox. The checkbox column has fixed width.
                checkboxHeader: false,//设置false 将在列头隐藏check-all checkbox .
                singleSelect: false,//true复选框只能选择一条记录
                //idField: ID,//指定主键列
                clickToSelect: false,//true点击行
                toolbar: '#toolbar',    //工具按钮用哪个容器
                sortName: 'a.UpdateTime',
                sortOrder: 'desc',
                sortStable: true,
                onRefresh: function () {
                    $.jun.spinner.spin($('body').get(0));
                },
                onLoadSuccess: function (data) {
                    if ($('#keyword').length > 0) {
                        $("body").mark($('#keyword').val());
                    }
                    $.jun.spinner.spin();
                },
                onLoadError: function (data) {
                    console.log("load error");
                    $.jun.spinner.spin();
                },
                onClickRow: function (row) {
                    //点击行
                },
                onSearch: function (row) {
                },
                columns: [
                {
                    field: 'id',
                    title: '编号',
                    visible: false
                },
                {
                    field: 'name',
                    title: '姓名',
                    width: 250,
                    align: 'center',
                    valign: 'middle',
                    sortable: false,
                    titleTooltip: '名称',
                    formatter: function (value, row, index) {
                        var a = utils.formatString('<a title="{0}" target="_blank"  href="javascript:;" onclick="$.jun.Server_detail(\'{1}\')">{2}</a>', (row["description"] != '') ? row["description"] : row["name"], row["id"], row["name"].cutStr(30));
                        return a;
                    }
                },
                {
                    field: 'level',
                    title: '等级',
                    width: 50,
                    align: 'center',
                    valign: 'middle',
                    sortable: false,
                    formatter: function (name) {
                        return name;
                    }
                },
                {
                    field: 'type',
                    title: '类型',
                    width: 50,
                    align: 'center',
                    valign: 'middle',
                    sortable: false,
                    formatter: function (name) {
                        return name;
                    }
                },
                {
                    field: 'url',
                    title: 'URL',
                    width: 450,
                    align: 'center',
                    valign: 'middle',
                    sortable: false,
                    formatter: function (name) {
                        var a = utils.formatString('<a title="{0}" target="_blank" href="{1}">{2}</a>', name, name, name.cutStr(200));
                        return a;
                    }
                },
                {
                    field: 'user',
                    title: '用户名',
                    width: 50,
                    align: 'center',
                    valign: 'middle',
                    sortable: false,
                    formatter: function (name) {
                        return name;
                    }
                },
                {
                    field: 'content',
                    title: '内容',
                    width: 50,
                    align: 'center',
                    valign: 'middle',
                    sortable: false,
                    formatter: function (name) {
                        return name.cutStr(30);
                    }
                }
                //,
                // {
                //     field: 'description',
                //     title: '描述',
                //     width: 50,
                //     align: 'center',
                //     valign: 'middle',
                //     sortable: false,
                //     formatter: function (name) {
                //         return name.cutStr(30);
                //     }
                // }
                ],
            });
        };
        $.jun.ServerTableInit();
        $('#btnAdd').click(function () {
            $.jun.Server_detail('0');
        });
        $('#btnKeyword').click(function () {
            $('#btnSearch').click();
        });
        $('#btnSearch').click(function () {
            $("#serverTable").bootstrapTable('refresh');

        });
        $('#tool input').keyup(function (event) {
            var keycode = event.which;
            if (keycode == 13) {
                $('#btnSearch').click();

            }
        });
        $('#tool select').change(function (event) {
            $('#btnSearch').click();
        });
        $('#toolToMore').click(function () {
            $('#toolKeyword').slideUp();
            $('#toolDetail').slideDown();
        });
        $('#btnToKeywod').click(function () {
            $('#toolKeyword').slideDown();
            $('#toolDetail').slideUp();
        });
        $('#keyword').focus().select();
    };
    //Server 编辑
    var handleServerEdit = function (param) {
        $('#btnSave').click(function () {
            $('#fm').ajaxSubmit(function (json) {
                layer.msg('更新成功', { icon: 1, time: 100 }, function () {
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
        $('#btnAdd').click(function () {
            $('#fm').find('[name=id]').val('');
            $('#fm').ajaxSubmit(function (json) {
                layer.msg('创建成功', { icon: 1, time: 100 }, function () {
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
        $('#btnDelete').click(function () {
            var id = $('#id').val();
            $.post('/ServerData/Delete', { id: id }, function (json) {
                layer.msg('删除成功', {
                    icon: 1, time: 100
                }, function () {
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
    };
    //Information列表
    var handleInformationIndex = function (param) {
        //列表搜索条件
        $.jun.Infomation_Searchcondition = function () {
            var form = $('#tool');
            var data = {
                level: form.find('[name="level"]').val(),
                logic: form.find('input[name="logic"]').is(':checked') ? "or" : "and",//包含任意关键字
                keyword: form.find('[name="keyword"]').val()
            };
            return data;
        };
        $.jun.Infomation_QueryParams = function (params) {
            var data = $.jun.Infomation_Searchcondition();
            data.limit = params.limit;
            data.offset = params.offset;
            data.sort = params.sort;
            data.sortOrder = params.sortOrder;
            data.search = params.search;
            return data;
        };
        $.jun.Infomation_detail = function (id) {
            layer.open({
                title: '新增导航',
                type: 2,
                area: ['1200px', '660px'],
                fixed: true,
                maxmin: true,
                content: '/Information/edit/' + id + ''
            });
        };
        $.jun.InformationTableInit = function () {
            $.jun.spinner.spin($('body').get(0));
            $("#InformationTable").bootstrapTable({
                method: 'post',
                url: "/InformationData/RetrievePaged",
                idField: 'ID',
                striped: true,//true隔行变色
                pagination: true,//true表格底部显示分页条
                pageList: [10, 20, 30, 40, 50],
                search: false,//true启用搜索框
                cache: false,//false使用缓存
                sidePagination: "server", //服务端请求
                queryParams: $.jun.Infomation_QueryParams,
                pageSize: 20,//每页条数
                pageNumber: 1,//初始页码
                clickToSelect: true,
                singleSelect: true,
                showHeader: true,//true显示列头
                showFooter: false,//true显示列脚
                showColumns: false,//是否显示内容下拉框
                showRefresh: false,//是否显示刷新按钮
                showToggle: false,//是否显示切换视图按钮
                checkbox: false,//True to show a checkbox. The checkbox column has fixed width.
                checkboxHeader: false,//设置false 将在列头隐藏check-all checkbox .
                singleSelect: false,//true复选框只能选择一条记录
                //idField: ID,//指定主键列
                clickToSelect: false,//true点击行
                toolbar: '#toolbar',    //工具按钮用哪个容器
                sortName: 'a.UpdateTime',
                sortOrder: 'desc',
                sortStable: true,
                onRefresh: function () {
                    $.jun.spinner.spin($('body').get(0));
                },
                onLoadSuccess: function (data) {
                    if ($('#keyword').length > 0) {
                        $("body").mark($('#keyword').val());
                    }
                    $.jun.spinner.spin();
                },
                onLoadError: function (data) {
                    console.log("load error");
                    $.jun.spinner.spin();
                },
                onClickRow: function (row) {
                    //点击行
                },
                onSearch: function (row) {
                },
                columns: [
                {
                    field: 'id',
                    title: '编号',
                    visible: false
                },
                {
                    field: 'name',
                    title: '姓名',
                    width: 250,
                    align: 'center',
                    valign: 'middle',
                    sortable: false,
                    titleTooltip: '名称',
                    formatter: function (value, row, index) {
                        var a = utils.formatString('<a title="{0}" target="_blank" href="javascript:;" onclick="$.jun.Infomation_detail(\'{1}\')">{2}</a>', row["name"], row["id"], row["name"].cutStr(30));
                        return a;
                    }
                },
                {
                    field: 'level',
                    title: '等级',
                    width: 100,
                    align: 'center',
                    valign: 'middle',
                    sortable: false,
                    formatter: function (name) {
                        return name;
                    }
                },
                {
                    field: 'type',
                    title: '类型',
                    width: 100,
                    align: 'center',
                    valign: 'middle',
                    sortable: false,
                    formatter: function (name) {
                        return name;
                    }
                },
                {
                    field: 'url',
                    title: 'URL',
                    width: 200,
                    align: 'center',
                    valign: 'middle',
                    sortable: false,
                    formatter: function (name) {
                        var a = utils.formatString('<a title="{0}" target="_blank" href="{1}">{2}</a>', name, name, name.cutStr(30));
                        return a;
                    }
                },
                {
                    field: 'user',
                    title: '用户名',
                    width: 200,
                    align: 'center',
                    valign: 'middle',
                    sortable: false,
                    formatter: function (name) {
                        return name;
                    }
                },
                {
                    field: 'content',
                    title: '内容',
                    width: 200,
                    align: 'center',
                    valign: 'middle',
                    sortable: false,
                    formatter: function (name) {
                        return name.cutStr(30);
                    }
                },
                 {
                     field: 'description',
                     title: '描述',
                     width: 200,
                     align: 'center',
                     valign: 'middle',
                     sortable: false,
                     formatter: function (name) {
                         return name.cutStr(30);
                     }
                 }
                ],
            });
        };
        $.jun.InformationTableInit();
        $('#btnAdd').click(function () {
            $.jun.Infomation_detail('0');
        });
        $('#btnKeyword').click(function () {
            $('#btnSearch').click();
        });
        $('#btnSearch').click(function () {
            $("#InformationTable").bootstrapTable('refresh');

        });
        $('#tool input').keyup(function (event) {
            var keycode = event.which;
            if (keycode == 13) {
                $('#btnSearch').click();

            }
        });
        $('#tool select').change(function (event) {
            $('#btnSearch').click();
        });
        $('#toolToMore').click(function () {
            $('#toolKeyword').slideUp();
            $('#toolDetail').slideDown();
        });
        $('#btnToKeywod').click(function () {
            $('#toolKeyword').slideDown();
            $('#toolDetail').slideUp();
        });
    };
    //Information 编辑
    var handleInformationEdit = function (param) {
        $('#btnSave').click(function () {
            $('#fm').ajaxSubmit(function (json) {
                layer.msg('更新成功', { icon: 1, time: 100 }, function () {
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
        $('#btnAdd').click(function () {
            $('#fm').find('[name=id]').val('');
            $('#fm').ajaxSubmit(function (json) {
                layer.msg('创建成功', { icon: 1, time: 100 }, function () {
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
        $('#btnDelete').click(function () {
            var id = $('#id').val();
            $.post('/InformationData/Delete', { id: id }, function (json) {
                layer.msg('删除成功', {
                    icon: 1, time: 100
                }, function () {
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
    };
    //Txt_列表
    var handleTxtIndex = function (param) {
        var west_size = 300;
        if (typeof $.cookie('west__size') != 'undefined' && $.cookie('west__size') != null) {
            west_size = $.cookie('west__size');
        }
        var myLayout = $("body").layout({
            initClosed: false,
            west__togglerContent_open: "&#8249;",
            west__togglerContent_closed: "&#8250;",
            togglerLength_open: 100,
            togglerLength_closed: 200,
            togglerTip_open: "鼠标点击按钮,隐藏左侧菜单",
            togglerTip_closed: "鼠标点击按钮,显示左侧菜单",
            togglerClass: 'ui-layout-toggler',
            sliderTip: "显示/隐藏侧边栏",
            sliderCursor: "pointer",
            slideTrigger_open: "mouseover",
            slideTrigger_close: "mouseout",
            west__size: west_size,
            south__size: 0,
            west__spacing_open: 5,
            west__spacing_closed: 8,
            north__spacing_open: 0,
            north__spacing_closed: 0,
            togglerAlign_open: "center",
            togglerAlign_closed: "center",
            west__resizable: true,
            north__closable: true,
            resizerTip: "鼠标按住拖动,可调整大小",
            center__onresize: function () {
                $.cookie('west__size', $('.ui-layout-west').width() + 22.5);
            },
            minSize: 100,
            maxSize: 800,
            closable: true,
            resizable: true,
            west__initClosed: true
        });
        $.jun.loadTag = function () {
            $.post('/TxtData/RetrieveTag', null, function (json) {
                var tag = $('#tag').empty();
                $.each(json, function (index, item) {
                    tag.append('<label><input  type="checkbox" name="tag" value="' + item.id + '" data-labelauty="' + item.title + "(" + item.txtCount + ")" + '"/></label>');
                });

                var aAdd = $('<a class="btn btn-info btn-xs" style="margin-top:-20px;margin-right:3px;" data-toggle="popover"><span class="glyphicon glyphicon-plus"></span></a>');
                var aConfirm = $('<a class="btn btn-success btn-sm"><span class="glyphicon glyphicon-ok"></span></a>');
                var input = $('<input class="" type="text" style="height:30px;"/>');
                var divPop = $('<div></div>'); divPop.append(input).append(aConfirm);
                aAdd.popover({
                    trigger: 'click',
                    placement: 'bottom', //top, bottom, left or right
                    title: '添加标签',
                    html: 'true',
                    content: divPop,
                });
                aConfirm.click(function () {
                    $.post('/txtData/AddTag', { tag: input.val() }, function () {
                        $.jun.loadTag();
                    });
                });
                var aReload = $('<a class="btn btn-info btn-xs" style="margin-top:-20px;margin-right:3px;"><span class="glyphicon glyphicon-refresh"></span></a>');
                aReload.click(function () {
                    $.post('/txtData/ReloadTag', {}, function () {
                        location.reload();
                    });
                });
                var aDelete = $('<a class="btn btn-danger btn-xs" href="/txt/deletetag" target="_blank"  style="margin-top:-20px;"><span class="glyphicon glyphicon-minus"></span></a>');
                tag.append(aReload).append(aAdd).append(aDelete);
                $('#tag :input[type="checkbox"]').labelauty({});
            });
        };
        $.jun.loadNav = function () {
            $.post('/TxtData/RetrieveType', {}, function (json) {
                var div = $('#navArea').empty();
                $.each(json, function (index, item) {
                    //var a = $('<a></a>');
                    var li = $('<li style="cursor:pointer;">' + item.title + '<span class="pull-right badge">' + item.txtCount + '<span>' + '</li>');// li.append(a);
                    //if (index == 0) {
                    //    li.addClass('selected');
                    //}
                    li.click(function () {
                        $('#txttype').val(item.id);
                        $("#TxtTable").bootstrapTable('refresh');
                        div.find('li').removeClass('selected');
                        li.addClass('selected');
                    });
                    div.append(li);
                });
                //显示全部
                var liAll = $('<li style="cursor:pointer;" class="selected">全部</li>');// li.append(a);
                liAll.click(function () {
                    $('#txttype').val("-1");
                    $("#TxtTable").bootstrapTable('refresh');
                    div.find('li').removeClass('selected');
                    liAll.addClass('selected');
                });
                div.append(liAll);
                //刷新数量 & 添加分类  & 删除分类
                var liTool = $('<li></li>');
                var aReCount = $('<a  class="btn btn-info btn-xs" style="margin-right:3px;" ><span class="glyphicon glyphicon-refresh"></span></a>');
                liTool.append(aReCount);
                aReCount.click(function () {
                    $.post('/txtdata/ReCount', {}, function () {
                        $.jun.loadNav();
                    });
                });
                var aAdd = $('<a class="btn btn-info btn-xs"  style="margin-right:3px;" data-toggle="popover"><span class="glyphicon glyphicon-plus"></span></a>');
                var aConfirm = $('<a class="btn btn-success btn-sm"><span class="glyphicon glyphicon-ok"></span></a>');
                var input = $('<input class="" type="text" style="height:30px;"/>');
                var divPop = $('<div></div>'); divPop.append(input).append(aConfirm);
                aAdd.popover({
                    trigger: 'click',
                    placement: 'bottom', //top, bottom, left or right
                    title: '添加分类',
                    html: 'true',
                    content: divPop,
                });
                aConfirm.click(function () {
                    $.post('/txtData/AddType', { type: input.val() }, function () {
                        $.jun.loadNav();
                    });
                });
                liTool.append(aAdd);
                liTool.append('<a class="btn btn-danger btn-xs" href="/txt/deletetype"  target="_blank"   style="margin-right:3px;"><span class="glyphicon glyphicon-minus"></span></a>');
                div.append(liTool);
            });
        };
        $.jun.loadTag();
        $.jun.loadNav();
        //列表搜索条件
        $.jun.Txt_Searchcondition = function () {
            var form = $('#tool');
            var tagarr = [];
            $('#tag').find('input[name="tag"]:checked').each(function () {
                tagarr.push($(this).val());
            });
            var data = {
                level: form.find('[name="level"]').val(),
                logic: form.find('input[name="logic"]').is(':checked') ? "or" : "and",//包含任意关键字
                keyword: form.find('[name="keyword"]').val(),
                logiccontent: form.find('[name="logiccontent"]').is(":checked") ? "content" : "title",//是否搜索全部
                tag: tagarr.join(','),//根据标签搜索
                txttype: $('#txttype').val()
            };
            return data;
        };
        $.jun.Txt_QueryParams = function (params) {
            var data = $.jun.Txt_Searchcondition();
            data.limit = params.limit;
            data.offset = params.offset;
            data.sort = params.sort;
            data.sortOrder = params.sortOrder;
            data.search = params.search;
            return data;
        };
        $.jun.Txt_detail = function (id) {
            var index = layer.open({
                title: '详情',
                type: 2,
                area: 'auto',
                //area: ['1200px', '660px'],
                fixed: true,
                maxmin: true,
                content: '/Txt/edit/' + id + ''
            });
            layer.full(index);
            //layer.full();
        };
        $.jun.TxtTableInit = function () {
            $.jun.spinner.spin($('body').get(0));
            $("#TxtTable").bootstrapTable({
                method: 'post',
                url: "/TxtData/RetrievePaged",
                idField: 'ID',
                striped: true,//true隔行变色
                pagination: true,//true表格底部显示分页条
                pageList: [10, 20, 30, 40, 50],
                search: false,//true启用搜索框
                cache: false,//false使用缓存
                sidePagination: "server", //服务端请求
                queryParams: $.jun.Txt_QueryParams,
                pageSize: 20,//每页条数
                pageNumber: 1,//初始页码
                clickToSelect: true,
                singleSelect: true,
                showHeader: true,//true显示列头
                showFooter: false,//true显示列脚
                showColumns: false,//是否显示内容下拉框
                showRefresh: false,//是否显示刷新按钮
                showToggle: false,//是否显示切换视图按钮
                checkbox: true,//
                checkboxHeader: true,
                singleSelect: false,//true复选框只能选择一条记录
                //idField: ID,//指定主键列
                clickToSelect: false,//true点击行
                toolbar: '#toolbar',    //工具按钮用哪个容器
                sortName: 'a.UpdateTime',
                sortOrder: 'desc',
                sortStable: true,
                onRefresh: function () {
                    $.jun.spinner.spin($('body').get(0));
                },
                onLoadSuccess: function (data) {
                    if ($('#keyword').length > 0) {
                        $("body").mark($.trim($('#keyword').val()));
                    }
                    $.jun.spinner.spin();
                },
                onLoadError: function (data) {
                    console.log("load error");
                    $.jun.spinner.spin();
                },
                onClickRow: function (row) {
                    //点击行
                },
                onSearch: function (row) {
                },
                columns: [
                    {
                        field: 'state',
                        checkbox: true
                    },
                {
                    field: 'id',
                    title: '编号',
                    width: 8,
                    //visible: false
                },
                   {
                       field: 'title',
                       title: '标题',
                       width: 400,
                       align: 'left',
                       valign: 'middle',
                       sortable: false,
                       titleTooltip: '名称',
                       formatter: function (value, row, index) {
                           var a = utils.formatString('<a title="{0}" target="_blank" href="javascript:;" onclick="$.jun.Txt_detail(\'{1}\')">{2}</a>', row["title"], row["id"], row["title"].cutStr(100));
                           return a;
                       }
                   },
                //{
                //    field: 'CreateTime',
                //    title: '创建时间',
                //    width: 100,
                //    align: 'center',
                //    valign: 'middle',
                //    sortable: false,
                //    formatter: function (value, row, index) {
                //        return $.jun.formatDateTime(row["CreateTime"]);
                //    }
                //},
                  {
                      field: 'UpdateTime',
                      title: '更新时间',
                      width: 100,
                      align: 'center',
                      valign: 'middle',
                      sortable: false,
                      formatter: function (value, row, index) {
                          return $.jun.formatDateTime(row["UpdateTime"]);
                      }
                  }
                ],
            });
        };
        $.jun.TxtTableInit();
        $('#btnAdd').click(function () {
            $.jun.Txt_detail('0');
        });
        $('#btnKeyword').click(function () {
            $("#TxtTable").bootstrapTable('refresh');
        });
        $('#tool input').keyup(function (event) {
            var keycode = event.which;
            if (keycode == 13) {
                $('#btnKeyword').click();
            }
        });
        $('#tool select').change(function (event) {
            $('#btnKeyword').click();
        });
        $('#keyword').focus().select();

    };
    //Txt_ 编辑
    var handleTxtEdit = function (param) {
        //KEDITOR.instances.content.setData($('#html').val());
        $.jun.loadTag = function (selected) {
            var selectedArray = selected.split(',');
            $.post('/TxtData/RetrieveTag', null, function (json) {
                var tag = $('#tag');
                $.each(json, function (index, item) {
                    var label = $('<label></label>');
                    var cb = $('<input  type="checkbox" value="' + item.id + '" data-labelauty="' + item.title + '"/>');
                    label.append(cb);
                    tag.append(label);
                    $.each(selectedArray, function (index2, item2) {
                        if (item2 == item.id) {
                            cb.attr('checked', 'checked');
                        }
                    });
                });
                $('#tag :input[type="checkbox"]').labelauty({});
            });
        };
        $.jun.loadNav = function (selected) {
            $.post('/TxtData/RetrieveType', {}, function (json) {
                var div = $('#navArea');
                $.each(json, function (index, item) {
                    if (selected == item.id) {
                        div.append('<label><input checked="checked"  type="radio" name="type" value="' + item.id + '" data-labelauty="' + item.title + '"/></label>');
                    }
                    else {
                        div.append('<label><input   type="radio" name="type" value="' + item.id + '" data-labelauty="' + item.title + '"/></label>');
                    }
                });
                $('#navArea :input[type="radio"]').labelauty({});
            });
        };
        $.jun.loadTag($('#hidTag').val());
        $.jun.loadNav($('#hidTxtType').val());
        $('#btnCalcel').click(function () {
            var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
            parent.layer.close(index);
        });
        $('#btnSave').click(function () {
            var radio = $('#navArea :input[type="radio"]:checked').val();
            var selectedArr = [];
            $('#tag :input[type="checkbox"]:checked').each(function () {
                selectedArr.push($(this).val());
            });
            $('#hidTxtType').val(radio);
            $('#hidTag').val(selectedArr.join(','));

            var htmlStr = CKEDITOR.instances.html.getData();    //获取textarea的值  
            var contentStr = CKEDITOR.instances.html.document.getBody().getText();    //获取textarea的值  
            $('#html').val(htmlStr);
            $('#content').val(contentStr);
            $('#fm').ajaxSubmit(function (json) {
                layer.msg('更新成功', { icon: 1, time: 100 }, function () {
                    //$("#btnKeyword", parent.document).click();
                    parent.location.reload();
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
        $('#btnAdd').click(function () {
            $('#fm').find('[name=id]').val('');
            var radio = $('#navArea :input[type="radio"]:checked').val();
            var selectedArr = [];
            $('#tag :input[type="checkbox"]:checked').each(function () {
                selectedArr.push($(this).val());
            });
            $('#hidTxtType').val(radio);
            $('#hidTag').val(selectedArr.join(','));
            var htmlStr = CKEDITOR.instances.html.getData();    //获取textarea的值  
            var contentStr = CKEDITOR.instances.html.document.getBody().getText();    //获取textarea的值  
            $('#html').val(htmlStr);
            $('#content').val(contentStr);
            $('#fm').ajaxSubmit(function (json) {
                layer.msg('创建成功', { icon: 1, time: 100 }, function () {
                    $("#btnKeyword", parent.document).click();
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
        $('#btnDelete').click(function () {
            var id = $('#id').val();
            $.post('/TxtData/Delete', { id: id }, function (json) {
                layer.msg('删除成功', {
                    icon: 1, time: 100
                }, function () {
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
    };
    //导航 编辑
    var handleNavEdit = function (param) {
        $('#btnSave').click(function () {
            $('#fmNavigationEdit').ajaxSubmit(function (json) {
                layer.msg('保存成功', { icon: 1, time: 100 }, function () {
                    $("#btnLoadNav", parent.document).click();
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
        //$('#btnAdd').click(function () {
        //    $('#fmNavigationEdit').find('[name=id]').val('');
        //    $('#fmNavigationEdit').ajaxSubmit(function (json) {
        //        layer.msg('创建成功', { icon: 1, time: 100 }, function () {
        //            var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
        //            parent.layer.close(index);
        //        });
        //    });
        //});
    };
    //导航列表
    var handleNavList = function (param) {
        $.jun.loadNav();
        $('#btnKeyword').click(function () {
            $("#nav").mark($('#keyword').val());
        });
        $('#keyword').keyup(function (event) {
            var keycode = event.which;
            if (keycode == 13) {
                $('#btnKeyword').click();
            }
        })
        $('#keyword').focus().select();
    };
    //联系方式列表
    var handlePeopleIndex = function (param) {
        var getDom = function (json) {
            var tb = $('<table class="tb"></table>');
            var head = $('<tr></tr>');
            head.append('<th> ID </th>');
            head.append('<th> Relationship </th>');
            //head.append('<th> RelationshipCompany </th>');
            head.append('<th> Chinese Name </th>');
            //head.append('<th> EnglishName </th>');
            //head.append('<th> NickName </th>');
            //head.append('<th> MarriageTime </th>');
            head.append('<th> Address Registered </th>');
            //head.append('<th> Address_Post </th>');
            //head.append('<th> Remarks </th>');
            head.append('<th> Phone </th>');
            head.append('<th> Email </th>');
            //head.append('<th> QQ </th>');
            //head.append('<th> IDNumber </th>');
            head.append('<th> Gender </th>');
            //head.append('<th> Birthday </th>');
            //head.append('<th> Hobby </th>');
            //head.append('<th> CurrentCity </th>');
            //head.append('<th> PostalCode </th>');
            //head.append('<th> MobilePhone </th>');
            //head.append('<th> Homepage </th>');
            //head.append('<th> CreateTime </th>');
            //head.append('<th> Keyworks </th>');
            head.append('<th> UpdateTime </th>');
            tb.append(head);
            $.each(json, function (index, item) {
                var tr = $('<tr></tr>');
                tr.append('<td>' + item.ID + '</td>');
                tr.append('<td>' + item.Relationship + '</td>');
                //tr.append('<td>' + item.RelationshipCompany + '</td>');
                var td = $('<td><a>' + item.ChineseName + '</a></td>');
                td.click(function () {
                    layer.open({
                        title: '新增导航',
                        type: 2,
                        area: ['1200px', '660px'],
                        fixed: false,
                        maxmin: true,
                        content: '/people/edit?id=' + item.ID + ''
                    });
                });
                tr.append(td);
                //tr.append('<td>' + item.EnglishName + '</td>');
                //tr.append('<td>' + item.NickName + '</td>');
                //tr.append('<td>' + item.MarriageTime + '</td>');
                tr.append('<td>' + item.Address_Registered + '</td>');
                //tr.append('<td>' + item.Address_Post + '</td>');
                //tr.append('<td>' + item.Remarks + '</td>');
                tr.append('<td>' + item.Phone + '</td>');
                tr.append('<td>' + item.Email + '</td>');
                //tr.append('<td>' + item.QQ + '</td>');
                //tr.append('<td>' + item.IDNumber + '</td>');
                tr.append('<td>' + item.Gender + '</td>');
                //tr.append('<td>' + item.Birthday + '</td>');
                //tr.append('<td>' + item.Hobby + '</td>');
                //tr.append('<td>' + item.CurrentCity + '</td>');
                //tr.append('<td>' + item.PostalCode + '</td>');
                //tr.append('<td>' + item.MobilePhone + '</td>');
                //tr.append('<td>' + item.Homepage + '</td>');
                //tr.append('<td>' + item.CreateTime + '</td>');
                //tr.append('<td>' + item.Keyworks + '</td>');
                tr.append('<td>' + item.UpdateTime + '</td>');
                if (index % 2 == 0) {
                    tr.addClass('bg');
                }
                tr.hover(function () {
                    tr.addClass('hover');
                }, function () {
                    tr.removeClass('hover');
                });
                tb.append(tr);
            })
            return tb;
        };
        var getParam = function () {
            var param = {
            };
            var tool = $('#tool');
            param.keyword = tool.find('[name="keyword"]').val();
            if (tool.find("input[type='checkbox']").is(':checked')) {
                param.logic = "or";
            } else {
                param.logic = "and";
            }
            console.dir(param);
            return param;
        };
        $('#btnKeyword').click(function () {
            $.post('/PeopleData/Retrieve', getParam(), function (json) {
                $('#content').empty().append(getDom(json));
            });
        });
        $('#tool input').keyup(function (event) {
            var keycode = event.which;
            if (keycode == 13) {
                $('#btnKeyword').click();
            }
        })
        $('#btnAdd').click(function () {
            layer.open({
                title: '新增导航',
                type: 2,
                area: ['1200px', '660px'],
                fixed: false,
                maxmin: true,
                content: '/people/edit?id=0'
            });
        });
        $('#keyword').focus().select();
    };
    //联系方式编辑
    var handlePeopleEdit = function (param) {
        $('#btnSave').click(function () {
            $('#fm').ajaxSubmit(function (json) {
                layer.msg('更新成功', {
                    icon: 1, time: 100
                }, function () {
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
        $('#btnAdd').click(function () {
            $('#fm').find('[name=id]').val('');
            $('#fm').ajaxSubmit(function (json) {
                layer.msg('创建成功', {
                    icon: 1, time: 100
                }, function () {
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
        $('#btnDelete').click(function () {
            var id = $('#id').val();
            $.post('/PeopleData/Delete', { id: id }, function (json) {
                layer.msg('删除成功', {
                    icon: 1, time: 100
                }, function () {
                    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                    parent.layer.close(index);
                });
            });
        });
    };
    //格式化单元格日期时间
    $.jun.formatDateTime = function (obj) {
        if (typeof obj != 'undefined' && obj != null) {
            return obj.dateTimeFormat();
        }
        else {
            return '';
        }
    };
    $.jun.loadNav = function () {
        var nav = $('#nav');
        $.post('/navigationdata/Retrieve', {
        }, function (json) {
            console.dir(json);
            nav.empty();
            var type = json.type;
            var list = json.list;
            $.each(type, function (indexType, jsonType) {
                var li = $('<li></li>');
                li.append('<p class="bg-primary">' + jsonType.name + '</p>')
                $.each(list, function (index, item) {
                    if (item.type == jsonType.id) {
                        var a = $('<a class="btn btn-default btn-xs" href="' + item.url + '" alt="' + item.description + '"><span class="' + item.icon + '"></span>&nbsp;' + item.name + '</a>');
                        li.append(a)
                        a.contextify({
                            items: [
                                  {
                                      text: '编辑', onclick: function () {
                                          layer.open({
                                              title: '编辑导航',
                                              type: 2,
                                              area: ['1200px', '660px'],
                                              fixed: false,
                                              maxmin: true,
                                              content: '/navigation/edit/' + item.id + '?type=' + jsonType.id
                                          });
                                      }
                                  },
                                   {
                                       text: '删除', onclick: function () {
                                           $.post('/navigationdata/delete/' + item.id, {}, function (json) {
                                               if (json.IsSuccess) {
                                                   layer.msg('删除成功', {
                                                       icon: 1, time: 100
                                                   }, function () {
                                                       //var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
                                                       //parent.layer.close(index);
                                                   });
                                               }
                                           })
                                       }
                                   }
                            ]
                        });
                    }
                });
                var btnAdd = $('<a class="btn btn-info btn-xs"><span class="glyphicon glyphicon-plus"></span></a>');

                btnAdd.click(function () {
                    layer.open({
                        title: '新增导航',
                        type: 2,
                        area: ['1200px', '660px'],
                        fixed: false,
                        maxmin: true,
                        content: '/navigation/edit/0?type=' + jsonType.id
                    });
                });
                li.append(btnAdd);
                nav.append(li);
            });

        });
    };
    $.jun.spinner = new Spinner({
        innerImage: {
            url: '/Content/img/logo.png',
            width: 38,
            height: 38
        },
        lines: 16,
        length: 19,
        width: 5,
        radius: 20,
        corners: 1,
        rotate: 0,
        direction: 1,
        color: '#123',
        speed: 2.1,
        trail: 60,
        shadow: false,
        hwaccel: false,
        className: 'spinner',
        zIndex: 2e9,
        top: 'auto',
        left: 'auto',
        position: 'relative', // element position
        progress: true,      // show progress tracker
        progressTop: 0,       // offset top for progress tracker
        progressLeft: 0       // offset left for progress tracker
    });
    return {
        //Initialise theme pages
        init: function (param) {
            //导航
            if (App.isPage("Navigation_Index")) {
                handleNavList(param);
            }
            if (App.isPage("Navigation_Edit")) {
                handleNavEdit(param);
            }
            //Information
            if (App.isPage("Information_Index")) {
                handleInformationIndex(param);
            }
            if (App.isPage("Information_Edit")) {
                handleInformationEdit(param);
            }
            //Server
            if (App.isPage("Server_Index")) {
                handleServerIndex(param);
            }
            if (App.isPage("Server_Edit")) {
                handleServerEdit(param);
            }
            //联系方式
            if (App.isPage("People_Index")) {
                handlePeopleIndex(param);
            }
            if (App.isPage("People_Edit")) {
                handlePeopleEdit(param);
            }
            if (App.isPage("Txt_Index")) {
                handleTxtIndex(param);
            }
            if (App.isPage("Txt_Edit")) {
                handleTxtEdit(param);
            }

        },
        setPage: function (name) {
            currentPage = name;
            console.dir(currentPage);//todo::
        },
        //Is Page
        isPage: function (name) {
            return currentPage == name ? true : false;
        }
    }
}();