document.addEventListener('DOMContentLoaded', () => {
	const commonForm = new CommonForm(
		'articleForm',
		'articleFormSubmit',
		'Success create article. ',
		'/settings/articles/',
		'Error create article. ',
		showGeneralNotifyMessage,
	);
});