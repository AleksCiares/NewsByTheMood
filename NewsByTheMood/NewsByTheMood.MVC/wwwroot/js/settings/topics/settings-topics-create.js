document.addEventListener('DOMContentLoaded', () => {
	const commonForm = new CommonForm(
		'topicForm',
		'topicFormSubmit',
		'Success create topic. ',
		'/settings/topics/',
		'Error create topic. ',
		showGeneralNotifyMessage,
	);

	$(document).on("click", ".icon-item", function () {
		var clickedIconCssClass = $(this).attr("value");
		$("#TopicIconCssClass").attr("value", clickedIconCssClass);
	});
});