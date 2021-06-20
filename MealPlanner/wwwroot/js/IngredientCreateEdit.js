$(document).ready(function () {
    $('#Name').on('keyup', function () {
        var name = $(this).val();
        $.ajax({
            type: "GET",
            url: `/Ingredients/SearchForSimilar?name=${name}`,
            success: function (data) {
                if (data.success && data.existingIngredients.length > 0) {
                    $('#ExistingIngredientCheckContainer').show(200);
                    var ul = document.getElementById('ExistingIngredientList');
                    ul.innerHTML = '';
                    for (var i = 0; i < data.existingIngredients.length; i++) {
                        var li = document.createElement('li');
                        li.appendChild(document.createTextNode(data.existingIngredients[i]));
                        ul.appendChild(li);
                    }
                }
                else {
                    $('#ExistingIngredientCheckContainer').hide(200);
                }
            }
        });
    });
});