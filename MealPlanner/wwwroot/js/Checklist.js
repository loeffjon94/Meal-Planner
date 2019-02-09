function InitTextBoxUpdate() {
    $('.check-textbox').on('input change', function () {
        var id = $(this).parent('li').data('id');
        UpdateShoppingItemEntry(id, $(this).val());
    });
}

function AppendNewLine(id) {
    $(".ShoppingList").append('<li class="item" style="display:none;" data-id="' + id + '">' +
        '    <input type="checkbox" class="shoppingItemCheck" data-id="' + id + '" id="cb' + id + '">' +
        '    <label for="cb' + id + '" class="check-box"></label>' +
        '    <input maxlength="100" autofocus="autofocus" placeholder="Click to edit" value="" class="check-textbox">' +
        '    <a href="javascript:RemoveCheck(' + id + ')"><i class="fas fa-times text-danger"></i></a>' +
        '</li>');
    $('.ShoppingList').find('.item:last').slideDown(200, function () {
        $('.ShoppingList').find('.item:last').find('check-textbox').focus();
    });
    InitTextBoxUpdate();
    InitReorder();
}

function AddItem() {
    CreateShoppingItemEntry(function (id) {
        AppendNewLine(id);
    });
}

function RemoveCheck(id) {
    $('li[data-id="' + id + '"]').slideUp(200, function () {
        $('li[data-id="' + id + '"]').remove();
    });
    RemoveItem(id);
}

function InitReorder() {
    $('.ShoppingList').sortable().bind('sortupdate', function (event, ui) {
        var id = ui.item.data('id');
        var previousId = ui.item.prev().data('id');
        UpdateOrder(id, previousId);
    });
}

$(document).ready(function () {
    InitTextBoxUpdate();
    InitReorder();
});