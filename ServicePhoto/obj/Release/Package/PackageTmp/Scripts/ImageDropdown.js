(function ($) {
    $.fn.ImageDropdown = function (settings) {
        //Establish our default settings
        var settings = $.extend({
            selectedIndexChanged: function (val) { }
        }, settings);
        //Get Id
        var id = $(this).attr("id");
        var selectRef = $(this);
        //Create Div Tag
        var divRef = $(this).after("<div class='customCombobox' id='custCombo" + id + "'></div>").next();
        //Create ul and li
        var ulRef = $(divRef).after("<ul class='ulcustomCombobox' id='ul" + id + "'></ul>").next();
        var count = 1;
        //Add li tags
        $(this).children().each(function () {
            var bg = $(this).css('background-image').replace(/^url|[\(\)]/g, '');
            var htmltxt = "<li id='" + count + "'><a><img src=" + bg + " class='imgDisplay' value='" + $(this).attr("value") + "' /><p class='imageText'>" + $(this).html() + "</p></a></li>";
            $(ulRef).append(htmltxt);
            count += 1;
        });
        //Set Default Selected
        $(this).children().each(function () {
            var selected = $(this).attr('selected');
            if (selected == 'selected') {
                setDropdownValue($(selectRef), $(this).attr('value'))
            }
        });
        //Add Event Handlres
        //drpDown Image click
        divRef.click(drpDownClick);
        //li click
        $(ulRef).children().click(function () { liClick($(this), settings) });
        //Hide Default look
        $(this).hide();
    }
    //Get Current Dropdown Value
    $.fn.GetImageDropdownValue = function () {
        return $(this).next().find('.imgDisplay').attr('value');
    }
    //Set Dropdown Value
    $.fn.SetImageDropdownValue = function (val) {
        setDropdownValue(this, val);
    }

    function setDropdownValue(ref, val) {
        $(ref).next().next().children().each(function () {
            var curVal = $(this).find('.imgDisplay').attr('value');
            if (curVal == val) {
                $(ref).next().html($(this).html());
                $(ref).val(val);
            }
        })
    }
    //List item click event handler
    function liClick(ref, settings) {
        //Get div tag
        var cmbBox = $(ref).parent().prev();
        var currVal = $(cmbBox).find('.imgDisplay').attr('value');
        //Raise Event
        var newVal = $(ref).find('.imgDisplay').attr('value');
        if (currVal != newVal) {
            //Set div tag text/image
            cmbBox.html($(ref).html());
            settings.selectedIndexChanged($(ref).find('.imgDisplay').attr('value'));
            //Set Select Tag Value
            var selectTag = $(cmbBox).prev();
            $(selectTag).val(newVal);
        }
        //Hide Dropdown
        $(ref).parent().hide();
    }
    //Dropdown div tag click event
    function drpDownClick() {
        //Get ul tag
        var dropDwn = $(this).next();
        //Show Dropdown
        if (dropDwn.is(":visible"))
            dropDwn.hide();
        else
            dropDwn.show();
    }
    //On click outside dropdown
    $(document).on('click', function (e) {
        var element, evt = e ? e : event;
        if (evt.srcElement)
            element = evt.srcElement;
        else if (evt.target)
            element = evt.target;
        //Hide if clicked outside
        if (element.className != "customCombobox") {
            $("ul.ulcustomCombobox").hide();
        }
    });
}(jQuery));