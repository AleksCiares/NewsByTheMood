document.addEventListener("DOMContentLoaded", function () {
    let modelProperty = new ModelProperty("Tags", "#tags-multiple-select option", true, "getValueFromAttribute", "value");
    parseModelProperty(modelProperty, 'createArticleForm');
});