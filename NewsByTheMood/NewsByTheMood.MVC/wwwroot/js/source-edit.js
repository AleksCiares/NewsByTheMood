document.addEventListener("DOMContentLoaded", function () {
    var form = document.querySelector("form");
    form.addEventListener("submit", function () {
        var topics = document.querySelectorAll("#topics-single-select option");

        var tagsContainer = document.querySelector(".hidden-data");
        tagsContainer.innerHTML = ""; // Clear previous inputs

        topics.forEach(function (topic, index) {
            var input = document.createElement("input");
            input.type = "hidden";
            input.name = `Topics[${index}].Value`;
            input.value = topic.value;
            tagsContainer.appendChild(input);

            var input = document.createElement("input");
            input.type = "hidden";
            input.name = `Topics[${index}].Text`;
            input.value = topic.textContent;
            tagsContainer.appendChild(input);

            var input = document.createElement("input");
            input.type = "hidden";
            input.name = `Topics[${index}].Selected`;
            input.value = topic.selected;
            tagsContainer.appendChild(input);
        });
    });
});