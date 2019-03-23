function UpdateIngredientOrder(id, previousId, nextId) {
    $.ajax({
        type: "POST",
        url: '/Ingredients/UpdateIngredientOrder',
        data: { id: id, previousId: previousId, nextId: nextId },
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
        var id = ui.item[0].dataset.id;
        var previousSibling = ui.item[0].previousElementSibling;
        var previousId = null;
        if (previousSibling)
            previousId = previousSibling.dataset.id;
        var nextSibling = ui.item[0].nextElementSibling;
        var nextId = null;
        if (nextSibling)
            nextId = nextSibling.dataset.id;
        UpdateIngredientOrder(id, previousId, nextId);
    });
}

$(document).ready(function () {
    InitCheck();
});