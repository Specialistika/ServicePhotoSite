//initialize all ComboBoxes after DOM is fully loaded
$(function () {
    var $comboBoxs = $("span[ComboBox]"); //get all ComboBoxes. ComboBox is defined with span element that has ComboBox attribute

    //init each ComboBox with empty filter
    $comboBoxs.each(function (i, obj) {
        ComboBox_InitListData($(obj), "");
    });

    //add click event handler for ComboBox "V" button
    $comboBoxs.find(":button").on("click", function (e) {
        ComboBox_HideList(); //hide all ComboBoxes' dropdown list first

        var $container = $(this).closest("span"); //get the first element that matches the "span" selector, beginning at the current element and progressing up through the DOM tree
        ComboBox_InitListData($container, ""); //init ComboBox with empty filter
        $container.find("div").css({ "zIndex": "1", "visibility": "visible" }); //set css of div on zIndex and visibility
        e.stopPropagation(); //prevents the event from bubbling up the DOM tree, preventing any parent handlers from being notified of the event
    });

    //Set TextBox attribute and add keydown and keyup event handler for ComboBox textbox
    $comboBoxs.find(":text")
              .data("currentitem", -1) //current selected item is -1
              .attr("autocomplete", "off") //turn off auto complete
              .on("keydown", function (e) { //set keydown event handler
                  ComboBox_KeySelectItem(e);
              })
              .on("keyup", function (e) { //set keyup event handler
                  ComboBox_KeyFilterItem(e);
              });

    //add click event handler for document
    $(document).click(function (e) {
        var element = e.srcElement || e.target;
        if (element != undefined && element.tagName == "INPUT" && element.value == "V") {
            //when click on the ComboBox "V" button, then do nothing (note: event handler on "V" button will handle this event)
        }
        else {
            //when click on somewhere else, then hide all ComboBoxes' dropdown list
            ComboBox_HideList();
        }
    });
});

//init combobox items
function ComboBox_InitListData($container, filterValue) {
    var $div = $container.find("div");
    var newList = new Array();
    var oSelect = $container.find("select")[0];
    var len = oSelect.options.length;

    for (var i = 0; i < len; i++) {
        if (filterValue == undefined || filterValue == "") {
            newList[newList.length] = oSelect.options[i].text;
        }
        else {
            if (newList.length >= 9) {
                break;
            }
            var currVal = oSelect.options[i].text;
            if (currVal.length >= filterValue.length) {
                if (currVal.toLowerCase().substring(0, filterValue.length) == filterValue.toLowerCase()) {
                    newList[newList.length] = currVal;
                }
            }
        }
    }

    var sHtml = [];
    sHtml.push("<table border=\"0\" cellpadding=\"0\" cellspace=\"0\" width=\"100%\" border=\"1\" style=\"z-index:10; background-color:white;\">");
    for (var i = 0; i < newList.length; i++) {
        sHtml.push("<tr onMouseOver=\"this.bgColor='#191970'; this.style.color='#ffffff'; this.style.cursor='default'; \" onMouseOut=\"this.bgColor='#ffffff'; this.style.color='#000000';\">");
        sHtml.push("<td nowrap onClick=\"ComboBox_SelectItemWithMouse(this);\">");
        sHtml.push((newList[i] == "" ? "&nbsp;" : newList[i]));
        sHtml.push("</td>");
        sHtml.push("</tr>");
    }
    sHtml.push("</table>");

    $div.html(sHtml.join('')); //set the HTML contents of div to concatenated string from sHtml arrary

    $div.css("overflowY", "auto"); //oTmp.style.overflowY = "auto";
    $div.css("border", "1px solid midnightblue"); //oTmp.style.border = "1px solid midnightblue";
    $div.css("position", "absolute"); //oTmp.style.position = "absolute";
    $div.css("visibility", "hidden"); //oTmp.style.visibility = "hidden";

    var count = $container.find("table td").size(); //get total ComboBox items
    var $text = $container.find(":text"); //get textbox element
    var $button = $container.find(":button") //get button element
    $div.css("width", $button.outerWidth() + $button.offset().left - $text.offset().left); //make the dropdown list same width as textbox + "V" button
    if (count > 7 || count == 0) {
        $div.css({ "height": "150" }); //limit the height of dropdown list when there is more than 7 items or default the height of dropdown list when there is no item
    }
    else { 
        $div.css({ "height": count * 21 }); //set the height of dropdown box same as total of items' height. Each item's height is 21
    }
}

//hide ComboBox dropdown list
function ComboBox_HideList() {
    $("span[ComboBox]").find("div").css("visibility", "hidden");
}

//is ComboBox dropdown list hidden
function ComboBox_IsListHidden($container) {
    return $container.find("div").css("visibility") == "hidden";
}

//keydown
function ComboBox_KeySelectItem(e) {
    var txt = e.srcElement || e.target;
    var currentitem = $(txt).data("currentitem");
    var val = $.trim($(txt).val()); //get value in textbox
    var $container = $(txt).closest("span[ComboBox]"); //get ComboBox container that is defined with span element and ComboBox attribute

    var key = e.keyCode || e.which; //get key code
    switch (key) {
        case 38: //up
            if (val == "" || ComboBox_IsListHidden($container)) {
                return;
            }
            --currentitem;
            $(txt).data("currentitem", currentitem);
            ComboBox_ChangeItemWithKey(txt);
            break;
        case 40: //down
            if (val == "" || ComboBox_IsListHidden($container)) {
                return;
            }
            currentitem++;
            $(txt).data("currentitem", currentitem)
            ComboBox_ChangeItemWithKey(txt);
            break;
        case 13: //enter			
            if (!ComboBox_IsListHidden($container)) {
                ComboBox_HideList();
                return false;
            }
            break;
        case 27: //esc
            ComboBox_HideList();
            return false;
            break;
        default:
            break;
    }
}

//keyup
function ComboBox_KeyFilterItem(e) {
    var txt = e.srcElement || e.target;

    //do nothing if up, down, enter, esc key pressed
    if (e.keyCode == 38 || e.keyCode == 40 || e.keyCode == 13 || e.keyCode == 27) {
        return;
    }

    $(txt).data("currentitem", -1);
    var val = $(txt).val();
    if (val == "") {
        ComboBox_HideList();
        return;
    }

    var $container = $(txt).closest("span[ComboBox]");
    ComboBox_InitListData($container, val);

    var $div = $container.find("div");
    $div.css({ "zIndex": "1", "visibility": "visible" });

    //hide dropdown list if there is no item
    if ($div.find("td").size() == 0) {
        ComboBox_HideList();
    }
}

//change item
function ComboBox_ChangeItemWithKey(txt) {
    var $txt = $(txt);
    var currentitem = $txt.data("currentitem");
    var table = $txt.closest("span[ComboBox]").find("table")[0];

    for (i = 0; i < table.rows.length; i++) {
        table.rows[i].bgColor = "#ffffff";
        table.rows[i].style.color = "#000000";
    }
    if (currentitem < 0) {
        currentitem = table.rows.length - 1;
    }
    if (currentitem == table.rows.length) {
        currentitem = 0;
    }
    $txt.data("currentitem", currentitem);

    if (table.rows[currentitem] == null) {
        ComboBox_HideList();
        return;
    }
    table.rows[currentitem].bgColor = "#191970"; //darkblue color
    table.rows[currentitem].style.color = "#ffffff"; //whit color
    $txt.val($(table.rows[currentitem].cells[0]).text());
}

//select item
function ComboBox_SelectItemWithMouse(td) {
    var $div = $(td).closest("div");
    var $txt = $div.parent().find(":text");
    var selectedValue = $(td).text();
    if ($.trim(selectedValue) == "") {
        selectedValue = "";
    }
    $txt.val(selectedValue);
    $txt[0].focus();
    $div.css("visibility", "hidden");
}