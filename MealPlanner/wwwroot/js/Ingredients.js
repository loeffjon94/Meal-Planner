function UpdateIngredientOrder(id, previousId) {
    $.ajax({
        type: "POST",
        url: '/Ingredients/UpdateIngredientOrder',
        data: { id: id, previousId: previousId },
        success: function (data) {
            if (!data.success) {
                alert("Error: the ingredient may not have updated correctly.");
            }
        },
        error: function () {
            alert("Error: the ingredient may not have updated correctly.");
        }
    });
}

function InitCheck() {
    $('.sortableTable').sortable({
        handle: '.handler',
        helper: function (e, tr) {
            var $originals = tr.children();
            var $helper = tr.clone();
            $helper.children().each(function (index) {
                // Set helper cell sizes to match the original sizes
                $(this).width($originals.eq(index).width());
            });
            return $helper;
        }
    }).bind('sortupdate', function (event, ui) {
        var id = ui.item.data('id');
        var previousId = ui.item.prev().data('id');
        UpdateIngredientOrder(id, previousId);
    });
}

$(document).ready(function () {
    InitCheck();
});