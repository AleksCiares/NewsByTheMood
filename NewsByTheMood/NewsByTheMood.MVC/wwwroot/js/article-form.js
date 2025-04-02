document.addEventListener("DOMContentLoaded", function () {
    var form = document.querySelector("form");
    form.addEventListener("submit", function () {
        var tags = document.querySelectorAll("#tags-multiple-select option");
        var selectedTags = document.querySelectorAll("#tags-multiple-select option:checked");
        var sources = document.querySelectorAll("#sources-single-select option");

        var tagsContainer = document.querySelector(".hidden-data");
        tagsContainer.innerHTML = ""; // Clear previous inputs

        tags.forEach(function (tag, index) {
            var input = document.createElement("input");
            input.type = "hidden";
            input.name = `Tags[${index}].Value`;
            input.value = tag.value;
            tagsContainer.appendChild(input);

            var input = document.createElement("input");
            input.type = "hidden";
            input.name = `Tags[${index}].Text`;
            input.value = tag.textContent;
            tagsContainer.appendChild(input);

            var input = document.createElement("input");
            input.type = "hidden";
            input.name = `Tags[${index}].Selected`;
            input.value = tag.selected;
            tagsContainer.appendChild(input);
        });

        selectedTags.forEach(function (tag, index) {
            var input = document.createElement("input");
            input.type = "hidden";
            input.name = `Article.Tags[${index}].Value`;
            input.value = tag.value;
            tagsContainer.appendChild(input);

            var input = document.createElement("input");
            input.type = "hidden";
            input.name = `Article.Tags[${index}].Text`;
            input.value = tag.textContent;
            tagsContainer.appendChild(input);

            var input = document.createElement("input");
            input.type = "hidden";
            input.name = `Article.Tags[${index}].Selected`;
            input.value = tag.selected;
            tagsContainer.appendChild(input);
        });

        sources.forEach(function (tag, index) {
            var input = document.createElement("input");
            input.type = "hidden";
            input.name = `Sources[${index}].Value`;
            input.value = tag.value;
            tagsContainer.appendChild(input);

            var input = document.createElement("input");
            input.type = "hidden";
            input.name = `Sources[${index}].Text`;
            input.value = tag.textContent;
            tagsContainer.appendChild(input);

            var input = document.createElement("input");
            input.type = "hidden";
            input.name = `Sources[${index}].Selected`;
            input.value = tag.selected;
            tagsContainer.appendChild(input);
        });
    });
});

document.addEventListener('DOMContentLoaded', function () {
    var textarea = document.querySelector('#articleFormBody');
    if (textarea) {
        textarea.style.height = 'auto';
        textarea.style.height = (textarea.scrollHeight) + 'px';

        textarea.addEventListener('input', function () {
            this.style.height = 'auto';
            this.style.height = (this.scrollHeight) + 'px';
        });
    }

    var tagSearch = document.getElementById('articleFormTagsSearch');
    var tagSelect = document.getElementById('tags-multiple-select');

    tagSearch.addEventListener('input', function () {
        var filter = tagSearch.value.toLowerCase();
        var options = tagSelect.options;

        for (var i = 0; i < options.length; i++) {
            var option = options[i];
            var text = option.text.toLowerCase();

            if (text.includes(filter)) {
                option.style.display = '';
            } else {
                option.style.display = 'none';
            }
        }
    });
});