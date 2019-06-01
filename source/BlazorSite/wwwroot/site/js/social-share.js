function newPopup(url) {
    popupWindow = window.open(url, 'popUpWindow', 'height=300,width=400,resizable=yes,scrollbars=yes,toolbar=no,menubar=no,location=no,directories=no,status=no')
}

function copyStringToClipboard(str) {
    var el = document.createElement('textarea');
    el.value = str;
    
    el.setAttribute('readonly', '');
    el.style = { display: 'none' };
    document.body.appendChild(el);
    el.select();
    document.execCommand('copy');
    document.body.removeChild(el);
}