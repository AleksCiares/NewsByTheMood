document.addEventListener('DOMContentLoaded', function () {
    $(document).on("click", ".icon-item", function () {
        var clickedIconCssClass = $(this).attr("value");
        $("#TopicIconCssClass").attr("value", clickedIconCssClass)
    });

    var iconSearch = document.getElementById('iconSearch');
    var iconItems = document.querySelectorAll('.icon-item-wrap');

    iconSearch.addEventListener('input', function () {
        var filter = iconSearch.value.toLowerCase();

        iconItems.forEach(function (item) {
            var icon = item.querySelector(".icon-item").getAttribute("value").toLowerCase();
            if (icon.includes(filter)) {
                item.style.display = '';
            } else {
                item.style.display = 'none';
            }
        });
    });
});