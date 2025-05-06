document.addEventListener('DOMContentLoaded', () => {
	const commonForm = new CommonForm(
		'sourceForm',
		'sourceFormSubmit',
		'Success create source. ',
		'/settings/sources/',
		'Error create source. ',
		showGeneralNotifyMessage,
	);
});