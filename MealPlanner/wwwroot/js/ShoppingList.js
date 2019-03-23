function GetNumberOfShoppingItems() {
    $.get("/ShoppingList/NumberOfItems", function (data) {
        return data;
    });
}

function RemoveItem(id) {
    $.ajax({
        type: "POST",
        url: '/ShoppingList/RemoveShoppingItem',
        data: { id: id },
        success: function (data) {
            if (!data.success) {
                alert("Error: the item may not have been removed.");
            }
        },
        error: function () {
            alert("Error: the item may not have been removed.");
        }
    });
}

function CreateShoppingItemEntry(callback) {
    $.ajax({
        type: "POST",
        url: '/ShoppingList/CreateShoppingItem',
        success: function (data) {
            if (!data.id) {
                alert("Error: the item may not have created correctly.");
            }
            callback(data.id);
        },
        error: function () {
            alert("Error: the item may not have created correctly.");
        }
    });
}

function UpdateShoppingItemEntry(id, value) {
    $.ajax({
        type: "POST",
        url: '/ShoppingList/UpdateShoppingItem',
        data: { id: id, value: value },
        success: function (data) {
            if (!data.success) {
                alert("Error: the item may not have updated correctly.");
            }
        },
        error: function () {
            alert("Error: the item may not have updated correctly.");
        }
    });
}

function UpdateOrder(id, previousId) {
    $.ajax({
        type: "POST",
        url: '/ShoppingList/UpdateShoppingItemOrder',
        data: { id: id, previousId: previousId },
        success: function (data) {
            if (!data.success) {
                alert("Error: the item may not have updated correctly.");
            }
        },
        error: function () {
            alert("Error: the item may not have updated correctly.");
        }
    });
}

function InitCheck() {
    $('.shoppingItemCheck').change(function () {
        var id = $(this).data('id');
        $.ajax({
            type: "POST",
            url: '/ShoppingList/CheckShoppingItem',
            data: { id: id, checkedVal: $(this).is(':checked') },
            success: function (data) {
                if (!data.success) {
                    alert("Error: the item may not have been checked.")
                }
            },
            error: function () {
                alert("Error: the item may not have been checked.")
            }
        });
    });
}

$(document).ready(function () {
    InitCheck();
});