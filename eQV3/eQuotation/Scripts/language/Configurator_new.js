(function ($lang) {
    // 多國語系文字庫
    var gLib = {
        "cart": {
            "CN": "购物车",
            "US": "CART",
            "TW": "購物車",
            "EU": "CART",
            "DL": "CART"
        },
        "Continue": {
            "CN": "点击继续",
            "US": "Click to Continue",
            "TW": "點擊繼續",
            "EU": "Click to Continue",
            "DL": "Click to Continue"
        },
        "Collapse": {
            "CN": "全部闭合",
            "US": "Collapse All",
            "TW": "全部閉合",
            "EU": "Collapse All",
            "DL": "Collapse All"
         },
        "Expand": {
            "CN": "全部展开",
            "US": "Expand All",
            "TW": "全部展開",
            "EU": "Expand All",
            "DL": "Expand All"
        },
        "Continue_checkTitle": {
            "CN": "下列组件未选取:",
            "US": "Please select one component of category:",
            "TW": "下列組件未選取:",
            "EU": "Please select one component of category:",
            "DL": "Please select one component of category:"
        },
        "price": {
            "CN": "价格:",
            "US": "price:",
            "TW": "價格:",
            "EU": "price:",
            "DL": "price:"
        },
        "available": {
            "CN": "日期内有效:",
            "US": "Available on:",
            "TW": "日期內有效:",
            "EU": "Available on:",
            "DL": "Available on:"
        },
        "qty": {
            "CN": "数量:",
            "US": "Qty:",
            "TW": "數量:",
            "EU": "Qty:",
            "DL": "Qty:"
        },
        "TotalPrice": {
            "CN": "总计:",
            "US": "Total Price:",
            "TW": "總計:",
            "EU": "Total Price:",
            "DL": "Total Price:"
        }
    }

    // 文字庫管理器
    var gTxtLib = {
        resource: undefined,
        defaultSet: function (Language) {
            gTxtLib.set(gLib, Language);
            gTxtLib.updateUIText();
        },
        set: function (lib, lang) {
            var result = undefined;
            if (typeof lib == 'object') {
                result = {};
                for (var id in lib) {
                    var txtObj = lib[id];
                    result[id] = (lang in txtObj ? txtObj[lang] : undefined);
                }
            }
            this.resource = result;
        },
        has: function (id) {
            return (this.resource != undefined && id in this.resource);
        },
        get: function (id) {
            if (this.has(id)) return this.resource[id];
            return '---';
        },
        updateUIText: function () {
            $('.leng').each(function (index, element) {
                var $self = $(element),
                 this_id = $self.data('tid');
                if (gTxtLib.has(this_id)) {
                    $self.html(gTxtLib.get(this_id.toString()));
                }
            });
            $('.leng_val').each(function (index, element) {
                var $self = $(element),
                 this_id = $self.data('tid');
                if (gTxtLib.has(this_id)) {
                    $self.val(gTxtLib.get(this_id.toString()));
                }
            });
        }
    };
    $lang.gTxtLib = gTxtLib;

})($lang = window.$lang || {});