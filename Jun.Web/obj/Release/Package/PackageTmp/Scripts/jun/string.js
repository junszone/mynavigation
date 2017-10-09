//=====================在字符串原型上添加方法==============

//实现Contains方法（核心是用Index方法的返回值进行判断），代码如下所示：

//Judge current string contains substring or not
String.prototype.contains = function (subStr) {
    var currentIndex = this.indexOf(subStr);
    if (currentIndex != -1) {
        return true;
    }
    else {
        return false;
    }
}
//获取长度(汉字算两个，英文算一个)
String.prototype.gblen = function () {
    var len = 0;
    for (var i = 0; i < this.length; i++) {
        if (this.charCodeAt(i) > 127 || this.charCodeAt(i) == 94) {
            len += 2;
        } else {
            len++;
        }
    }
    return len;
}
//当字符串的长度大于指定len就截断,后面加上...
//create at 2016年10月17日 by jun
String.prototype.cutStr = function (len) {
    var str_length = 0;
    var str_len = 0;
    str_cut = new String();
    var _this = this;
    str_len = this.length;
    for (var i = 0; i < str_len; i++) {
        a = this.charAt(i);
        str_length++;
        if (escape(a).length >= 4) {
            //中文字符的长度经编码之后大于4
            str_length++;
        }
        str_cut = str_cut.concat(a);
        if (str_length >= len) {
            //console.log(_this+"_"+_this.gblen()+ "___" + len);
            if (_this.gblen() == len) {
                return str_cut
            } else {
                str_cut = str_cut.concat("...");
                return str_cut;
            }
        }
    }
    //如果给定字符串小于指定长度，则返回源字符串；
    if (str_length < len) {
        return this;
    }
}

// 返回字符的长度，一个中文算2个
String.prototype.chineseLength = function () {
    return this.replace(/[^\x00-\xff]/g, "**").length;
}
// 判断字符串是否以指定的字符串开始
String.prototype.startsWith = function (str) {
    return this.substr(0, str.length) == str;
}
// 判断字符串是否以指定的字符串结束
String.prototype.endsWith = function (str) {
    return this.substr(this.length - str.length) == str;
}
// 去掉字符两端的空白字符
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
// 去掉字符左端的的空白字符
String.prototype.trimStart = function () {
    return this.replace(/(^[\\s]*)/g, "");
}
// 去掉字符右端的空白字符
String.prototype.trimEnd = function () {
    return this.replace(/([\\s]*$)/g, "");
}
//显示器可视高度
String.prototype.screenClientHeight = function () {
    return document.body.clientHeight * parseFloat(this) + "px";
};
//显示器可视宽度
String.prototype.screenClientWidth = function () {
    return document.body.clientWidth * parseFloat(this) + "px";
};


//批量替换，比如：str.ReplaceAll([/a/g,/b/g,/c/g],["aaa","bbb","ccc"])     
String.prototype.replaceAll = function (A, B) {
    var C = this;
    for (var i = 0; i < A.length; i++) {
        C = C.replace(A[i], B[i]);
    };
    return C;
};

// 字符串从哪开始多长字符去掉     
String.prototype.remove = function (A, B) {
    var s = '';
    if (A > 0) s = this.substring(0, A);
    if (A + B < this.length) s += this.substring(A + B, this.length);
    return s;
};
//CSS加载
String.prototype.css3loading = function (A) {
    return '<h1><div class="spinner">  <div class="rect1"></div>  <div class="rect2"></div>  <div class="rect3"></div>  <div class="rect4"></div>  <div class="rect5"></div></div></h1><br/>';
}
//候选人来源.
String.prototype.cvSource = function () {
    var strlower = getString(this, '').toLowerCase();
    var flag = '';
    switch (strlower) {
        case 'input':
            flag = '手工填写';
            break;
        case 'upload':
            flag = '上传';
            break;
        case 'batchupload':
            flag = '批量上传';
            break;
        case 'listupload':
            flag = 'list上传';
            break;
        case 'emailcapture':
            flag = '邮件捕获';
            break;
        case 'password':
            flag = '密码';
            break;
        case 'theme':
            flag = '主题';
            break;
        case 'filecontent':
            flag = '简历原件';
            break;
        case 'filetxt':
            flag = '简历文本';
            break;
        case 'fileindex':
            flag = '上传文件列表';
            break;
        case 'cvhistory':
            flag = '简历列表';
            break;
        case 'cvlist':
            flag = '候选人列表';
            break;
        case 'linkedcandidatepagedlist':
            flag = "关联候选人分页列表";
            break;
        default:
            flag = strlower;
            break;
    }
    return flag;
}
//日志操作显示
String.prototype.logAction = function () {
    var flag = '';
    var nameLower = getString(this, '').toLocaleLowerCase();
    switch (nameLower) {
        case 'create':
            flag = '新增';
            break;
        case 'update':
            flag = '修改';
            break;
        case 'delete':
            flag = '删除';
            break;
        case 'pagedlist':
            flag = '查询列表';
            break;
        case 'detail':
            flag = '查看详情';
            break;
        case 'login':
            flag = '登录';
            break;
        case 'logout':
            flag = '退出登录';
            break;
        case 'logforced':
            flag = '其他地方登录';
            break;
        case 'import':
            flag = '导入';
            break;
        case 'export':
            flag = '导出';
            break;
        case 'parsing':
            flag = '解析';
            break;
        case 'preview':
            flag = '预览';
            break;
        case 'download':
            flag = '下载';
            break;
        case 'move':
            flag = '移动';
            break;
        case 'upload':
            flag = '上传';
            break;
        case 'toposition':
            flag = '推荐到职位';
            break;
        case 'tofolder':
            flag = '添加到文件夹';
            break;
        case 'batchaddcandidate':
            flag = '批量添加候选人';
            break;
        case 'addtag':
            flag = '打标签';
            break;
        case 'show':
            flag = "查看";
            break;
        case 'read':
            flag = "已读";
            break;
        case 'search':
            flag = "搜索";
            break;
        case 'removecandidate':
            flag = "移除候选人";
            break;
        case 'addcandidate':
            flag = "关联候选人";
            break;
        case 'capture':
            flag = "简历捕获";
            break;
        default:
            flag = nameLower;
            break;
    }
    return flag;
};
String.prototype.logObject = function () {
    var flag = '';
    var nameLower = getString(this, '').toLocaleLowerCase();
    switch (nameLower) {
        case 'cvinformation':
            flag = '候选人信息';
            break;
        case 'workexperience':
            flag = '工作经历';
            break;
        case 'education':
            flag = '教育经历';
            break;
        case 'certification':
            flag = '证书';
            break;
        case 'language':
            flag = '语言能力';
            break;
        case 'executivesummary':
            flag = '自我评价';
            break;
        case 'cvevent':
            flag = '联系记录';
            break;
        case 'user':
            flag = '用户信息';
            break;
        case 'email':
            flag = '用户邮箱';
            break;
        case 'calendar':
            flag = '日历提醒';
            break;
        case 'calendartitle':
            flag = '常用日历提醒';
            break;
        case 'advancedsearch':
            flag = '高级搜索';
            break;
        case 'emailsetting':
            flag = '邮箱设置';
            break;
        case 'cvflow':
            flag = '进展跟进';
            break;
        case 'tagscustom':
            flag = '个性化标签';
            break;
        case 'cvfile':
            flag = '文件';
            break;
        case 'cvinformationinitemail':
            flag = '新投递候选人';
            break;
        case 'candidate':
            flag = '候选人';
            break;
        case 'cvfolder':
            flag = '文件夹';
            break;
        case 'position':
            flag = '职位';
            break;
        case 'myhome':
            flag = "主页";
            break;
        case 'eweb':
            flag = "互联网";
            break;
        case 'plugin':
            flag = "Chrome插件";
            break;
        default:
            flag = nameLower;
            break;
    }
    return flag;
};
//文件格式
String.prototype.fileExtension = function () {
    var flag = '';
    var nameLower = getString(this, '').toLocaleLowerCase();
    switch (nameLower) {
        case '.xls': case '.xlsx':
            flag = '<img src="/Content/img/fileextension/xls.png" />';
            break;
        case '.doc': case '.docx':
            flag = '<img src="/Content/img/fileextension/doc.png" />';
            break;
        case '.pdf':
            flag = '<img src="/Content/img/fileextension/pdf.png" />';
            break;
        case '.html': case '.mht': case '.htm':
            flag = '<img src="/Content/img/fileextension/html.png" />';
            break;
        case '.txt':
            flag = '<img src="/Content/img/fileextension/txt.png" />';
            break;
        default:
            flag = '<img src="/Content/img/fileextension/txt.png" />';
            break;
    }
    return flag;
};
//json日期转化为现实的日期串
//\/Date(1492397309283)\/ -> 2017-04-17
String.prototype.dateFormat = function () {
    var date = this;
    var da = new Date(parseInt(date.replace("/Date(", "").replace(")/", "").split("+")[0]));
    return da.getFullYear() + "/" + (da.getMonth() + 1) + "/" + da.getDate();
};
//json日期转化为现实的日期时间串
//\/Date(1492397309283)\/ -> 2017-04-17 10:48:29
String.prototype.dateTimeFormat = function () {
    var date = this;
    var da = new Date(parseInt(date.replace("/Date(", "").replace(")/", "").split("+")[0]));
    //var weekdays = { 0: "星期日", 1: "星期一", 2: "星期二", 3: "星期三", 4: "星期四", 5: "星期五", 6: "星期六" }[da.getDay()];
    return da.getFullYear() + "/" + (da.getMonth() + 1) + "/" + da.getDate() + " " + da.getHours() + ":" + da.getMinutes() + ":" + da.getSeconds();
};
//json日期转化为现实的日期时间串，还包括星期
//\/Date(1492397309283)\/ -> 2017-04-17 星期一  10:48:29
String.prototype.dateTimeWeekFormat = function () {
    var date = this;
    var da = new Date(parseInt(date.replace("/Date(", "").replace(")/", "").split("+")[0]));
    var weekdays = { 0: "星期日", 1: "星期一", 2: "星期二", 3: "星期三", 4: "星期四", 5: "星期五", 6: "星期六" }[da.getDay()];
    return da.getFullYear() + "/" + (da.getMonth() + 1) + "/" + da.getDate() + " " + weekdays + " " + da.getHours() + ":" + da.getMinutes() + ":" + da.getSeconds();
};
//格式化日期
function DateTimeShow(object) {
    var str = getString(object, '');
    if (str.indexOf("1900") > -1) {
        return '';
    } else {
        return str;
    }
}
//判断空对象
//就是一个空对象，由于没有任何属性和方法，所以可以利用这一特点进行区别：
//isEmpty({}); // true
//isEmpty(null); // true
// console.log((typeof null)); //输出 object
// console.log(Object.prototype.toString.call(null));//输出 [object Null]
function isEmpty(object) {
    if (object === null || object === undefined) {
        return true;
    }
    for (var i in object) {
        // 存在属性或方法，则不是空对象
        return true;  // sodino.com
    }
    return false;
}
//把null或undefined或空的转化为''
//getString('abc','');//返回'abc'
//getString(null,'空');//返回'空'
function getString(object, defaultValue) {
    if (object === null || object === undefined) {
        return defaultValue;
    }
    for (var i in object) {
        // 存在属性或方法，则不是空对象
        return object;  // sodino.com
    }
    return defaultValue;
}

