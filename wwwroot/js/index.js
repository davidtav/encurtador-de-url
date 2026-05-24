const btnSubmit = document.getElementById("btnSubmit");
const btnCopy = document.getElementById("btnCopy");
const inputUrl = document.getElementById("url");
const urlResult = document.getElementById("urlResult");
const statusMessage = document.getElementById("statusMessage");

btnSubmit.addEventListener("click", function (e) {
  e.preventDefault();
  handleSubmitAsync();
});

inputUrl.addEventListener("keyup", function (evt) {
  if (evt.code === "Enter") {
    evt.preventDefault();
    handleSubmitAsync();
  }
});

btnCopy.addEventListener("click", async function () {
  const shortUrl = btnCopy.dataset.shortUrl;
  if (!shortUrl) {
    return;
  }

  try {
    await navigator.clipboard.writeText(shortUrl);
    setStatus("Link copiado para a área de transferência.", false);
  } catch {
    setStatus("Não foi possível copiar o link.", true);
  }
});

async function handleSubmitAsync() {
  const url = inputUrl.value.trim();

  if (!url) {
    setStatus("Informe uma URL para encurtar.", true);
    clearResult();
    return;
  }

  setLoading(true);

  const payload = { url: url };

  try {
    const apiResult = await fetch("/urls", {
      method: "post",
      body: JSON.stringify(payload),
      headers: {
        "content-type": "application/json",
      },
    });

    const json = await apiResult.json();

    if (apiResult.ok) {
      const shortUrl = json.shortUrl;
      urlResult.innerHTML = `<a href="${shortUrl}" target="_blank" rel="noopener noreferrer">${shortUrl}</a>`;
      btnCopy.dataset.shortUrl = shortUrl;
      btnCopy.disabled = false;
      setStatus("URL encurtada com sucesso.", false);
    } else {
      const message = json?.error?.message ?? "Falha ao encurtar URL.";
      setStatus(message, true);
      clearResult();
    }
  } catch {
    setStatus("Erro de conexão ao encurtar URL.", true);
    clearResult();
  } finally {
    setLoading(false);
  }
}

function setLoading(isLoading) {
  btnSubmit.disabled = isLoading;
  btnSubmit.textContent = isLoading ? "encurtando..." : "enter";
}

function setStatus(message, isError) {
  statusMessage.textContent = message;
  statusMessage.style.color = isError ? "#b00020" : "#1b5e20";
}

function clearResult() {
  urlResult.textContent = "";
  btnCopy.dataset.shortUrl = "";
  btnCopy.disabled = true;
}
