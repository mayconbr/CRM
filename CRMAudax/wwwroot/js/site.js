document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('*[alt]').forEach(element => element.removeAttribute('alt'));
});

function openModalMessage(message) {
    let modalMessage = document.querySelector('.modal-message')
    document.getElementById("divMessage").innerHTML = message;
    modalMessage.style.display = 'block';
}

function closeModalMessage(refresh = false) {
    var container = document.querySelector('.modal-message')
    container.style.display = "none"
    if (refresh) {
        window.location.reload();
    }
}

function openModalload(message) {
    debugger;
    let modalMessage = document.querySelector('.modal-load')
    document.getElementById("divMessage").innerHTML = message;
    modalMessage.style.display = 'block';
}

function closeModalload(refresh = false) {
    var container = document.querySelector('.modal-load')
    container.style.display = "none"
    if (refresh) {
        window.location.reload();
    }
}

function openModalMessageReprova(ClienteId) {
    let modalMessageReprova = document.querySelector('.modal-message-reprova')
    modalMessageReprova.style.display = 'block';

    document.getElementById("ClienteIdReprova").value = ClienteId;
}

function closeModalMessageReprova(refresh = false) {
    var container = document.querySelector('.modal-message-reprova')
    container.style.display = "none"
    if (refresh) {
        window.location.reload();
    }
}

function openModalMessageAprova(ClienteId) {
    let modalMessageReprova = document.querySelector('.modal-message-aprova')
    modalMessageReprova.style.display = 'block';

    document.getElementById("ClienteIdAprova").value = ClienteId;
}

function closeModalMessageAprova(refresh = false) {
    var container = document.querySelector('.modal-message-aprova')
    container.style.display = "none"
    if (refresh) {
        window.location.reload();
    }
}

//Coloca o Formulario dentro de um JSON
function parseForm(form) {
    var elements = form.elements;
    var obj = {};
    for (var i = 0; i < elements.length; i++) {
        var item = elements.item(i);
        if (typeof item === "object" &&
            typeof item.name !== "undefined" &&
            item.name != null && item.name.length > 0) {
            var item = elements.item(i);
            obj[item.name] = item.value;
        }
    }
    return obj;
}

// Criptografia em HASH
function sha1(n) {
    var v, t, d, f, i, r, u, o, s, c = function (n, t) {
        return n << t | n >>> 32 - t
    },
        a = function (n) {
            for (var i, r = "", t = 7; t >= 0; t--) i = n >>> 4 * t & 15, r += i.toString(16);
            return r
        },
        h = new Array(80),
        y = 1732584193,
        p = 4023233417,
        w = 2562383102,
        b = 271733878,
        k = 3285377520,
        e, l;
    for (n = unescape(encodeURIComponent(n)), e = n.length, l = [], t = 0; e - 3 > t; t += 4) d = n.charCodeAt(t) << 24 | n.charCodeAt(t + 1) << 16 | n.charCodeAt(t + 2) << 8 | n.charCodeAt(t + 3), l.push(d);
    switch (e % 4) {
        case 0:
            t = 2147483648;
            break;
        case 1:
            t = n.charCodeAt(e - 1) << 24 | 8388608;
            break;
        case 2:
            t = n.charCodeAt(e - 2) << 24 | n.charCodeAt(e - 1) << 16 | 32768;
            break;
        case 3:
            t = n.charCodeAt(e - 3) << 24 | n.charCodeAt(e - 2) << 16 | n.charCodeAt(e - 1) << 8 | 128
    }
    for (l.push(t); l.length % 16 != 14;) l.push(0);
    for (l.push(e >>> 29), l.push(e << 3 & 4294967295), v = 0; v < l.length; v += 16) {
        for (t = 0; 16 > t; t++) h[t] = l[v + t];
        for (t = 16; 79 >= t; t++) h[t] = c(h[t - 3] ^ h[t - 8] ^ h[t - 14] ^ h[t - 16], 1);
        for (f = y, i = p, r = w, u = b, o = k, t = 0; 19 >= t; t++) s = c(f, 5) + (i & r | ~i & u) + o + h[t] + 1518500249 & 4294967295, o = u, u = r, r = c(i, 30), i = f, f = s;
        for (t = 20; 39 >= t; t++) s = c(f, 5) + (i ^ r ^ u) + o + h[t] + 1859775393 & 4294967295, o = u, u = r, r = c(i, 30), i = f, f = s;
        for (t = 40; 59 >= t; t++) s = c(f, 5) + (i & r | i & u | r & u) + o + h[t] + 2400959708 & 4294967295, o = u, u = r, r = c(i, 30), i = f, f = s;
        for (t = 60; 79 >= t; t++) s = c(f, 5) + (i ^ r ^ u) + o + h[t] + 3395469782 & 4294967295, o = u, u = r, r = c(i, 30), i = f, f = s;
        y = y + f & 4294967295;
        p = p + i & 4294967295;
        w = w + r & 4294967295;
        b = b + u & 4294967295;
        k = k + o & 4294967295
    }
    return s = a(y) + a(p) + a(w) + a(b) + a(k), s.toLowerCase()
};

function sucessAtv(msg) {
    const message = document.getElementById('message');
    message.style.display = 'block';
    message.style.backgroundColor = '#70b12e';
    message.innerText = msg;

    setTimeout(() => {
        message.style.display = "none";
    }, 3500);
}

function ErrAtv(msg) {
    const message = document.getElementById('message');
    message.style.display = 'block';
    message.style.backgroundColor = '#bf0222';
    message.innerText = msg;

    setTimeout(() => {
        message.style.display = "none";
    }, 3500);
}

function openModalMessageQuatro(ClienteId) {
    debugger;
    let modalMessageReprova = document.querySelector('.modal-message-quatro')
    modalMessageReprova.style.display = 'block';

    document.getElementById("IdclienteEdit").value = ClienteId;
}

function closeModalMessageQuatro(refresh = false) {
    var container = document.querySelector('.modal-message-quatro')
    container.style.display = "none"
    if (refresh) {
        window.location.reload();
    }
}

function valifaForm(formId, ignoredNames) {
    // Obtém o formulário pelo ID
    var form = document.getElementById(formId);

    if (!form) {
        console.error('Formulário não encontrado!');
        return;
    }

    // Obtém todos os inputs dentro do formulário
    var inputs = form.getElementsByTagName('input');

    // Itera sobre todos os inputs
    for (var i = 0; i < inputs.length; i++) {
        var input = inputs[i];

        if (input.disabled || ignoredNames.includes(input.name)) {
            continue; // Pula os inputs desabilitados e passados como paramentro
        }

        // Verifica se o input está vazio
        if (input.value.trim() === '') {
            // Muda a cor de fundo do input vazio
            input.style.backgroundColor = '#eccbd0';
            input.style.border = '2px solid #BD4A5B';
        } else {
            // Restaura a cor de fundo original do input preenchido
            input.style.backgroundColor = '';
        }

    }
}

function doChangeContentArq() {
    const content = document.getElementById("content-arqv");
    const contentN = document.getElementById("contentNewArqv");
    const button = document.getElementById('rotateButton');

    // Verificar o estado atual do display
    const isContentVisible = content.style.display === "block" || content.style.display === "";

    // Alternar a visibilidade do conteúdo com animação
    if (isContentVisible) {
        content.classList.remove('fade-in');
        content.classList.add('fade-out');
        contentN.classList.remove('fade-out');
        contentN.classList.add('fade-in');

        setTimeout(() => {
            content.style.display = "none";
            contentN.style.display = "block";
            content.classList.remove('fade-out');
        }, 500); // Duração da animação
    } else {
        content.classList.remove('fade-out');
        content.classList.add('fade-in');
        contentN.classList.remove('fade-in');
        contentN.classList.add('fade-out');

        setTimeout(() => {
            content.style.display = "block";
            contentN.style.display = "none";
            contentN.classList.remove('fade-out');
        }, 500); // Duração da animação
    }

    // Alternar a rotação do botão
    button.classList.toggle('rotated');
}












