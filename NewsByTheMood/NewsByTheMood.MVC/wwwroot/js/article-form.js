
document.getElementById('tags-multiple-select').addEventListener('change', function () {
	var options = this.options;
	for (var i = 0; i < options.length; i++) {
		var option = options[i];
		var hiddenSelected = document.getElementsByName(`Tags[${i}].Selected`)[0];
		hiddenSelected.value = option.selected;
	}
});
