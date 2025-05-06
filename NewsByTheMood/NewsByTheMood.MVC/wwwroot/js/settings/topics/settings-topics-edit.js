document.addEventListener('DOMContentLoaded', () => {
	const commonForm = new CommonForm(
		'topicForm',
		'topicFormSubmit',
		'Success update topic. ',
		undefined,
		'Error update topic. ',
		showGeneralNotifyMessage,
	);

	$(document).on("click", ".icon-item", function () {
		var clickedIconCssClass = $(this).attr("value");
		$("#TopicIconCssClass").attr("value", clickedIconCssClass);
	});
});