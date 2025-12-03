#!/usr/bin/env bash
set -euo pipefail

# Unity DevOps передаёт:
# $1 — temp dir, $2 — artifacts dir (.build/last/default-ios), $3 — платформа
WORK_DIR="${1:-}"
ARTIFACTS_DIR="${2:-}"
PLATFORM="${3:-}"

echo "🚀 Uploading IPA to App Store Connect..."
echo "Artifacts: ${ARTIFACTS_DIR}"
echo "Work:      ${WORK_DIR}"
echo "Platform:  ${PLATFORM}"

# ---- 1) Находим IPA ----
IPA_PATH="${UNITY_PLAYER_PATH:-}"
if [[ -z "${IPA_PATH:-}" || ! -f "${IPA_PATH}" ]]; then
  IPA_PATH="$(find "${ARTIFACTS_DIR}" -type f -name '*.ipa' -print -quit || true)"
fi
if [[ -z "${IPA_PATH}" ]]; then
  IPA_PATH="$(find "${WORK_DIR}" -type f -name '*.ipa' -print -quit || true)"
fi
if [[ -z "${IPA_PATH}" ]]; then
  echo "❌ IPA not found in: ${ARTIFACTS_DIR} or ${WORK_DIR}"
  exit 1
fi
echo "📦 IPA: ${IPA_PATH}"

# ---- 2) Проверяем переменные окружения ----
: "${API_KEY_ID:?API_KEY_ID is not set}"
: "${API_ISSUER_ID:?API_ISSUER_ID is not set}"
: "${CONNECT_API_KEY:?CONNECT_API_KEY is not set}"

# ---- 3) Создаём p8 файл там, где ждёт altool ----
P8_DIR="${HOME}/.appstoreconnect/private_keys"
P8_PATH="${P8_DIR}/AuthKey_${API_KEY_ID}.p8"
mkdir -p "${P8_DIR}"

# Если в CONNECT_API_KEY нет строк BEGIN/END — добавим заголовки и переносы
if [[ "${CONNECT_API_KEY}" != *"BEGIN"* ]]; then
  # CONNECT_API_KEY хранится в одну строку, как мы и советовали
  printf '%s\n%s\n%s\n' \
    "-----BEGIN PRIVATE KEY-----" \
    "${CONNECT_API_KEY}" \
    "-----END PRIVATE KEY-----" > "${P8_PATH}"
else
  # Уже полноценный PEM — пишем как есть
  printf '%s\n' "${CONNECT_API_KEY}" > "${P8_PATH}"
fi

chmod 600 "${P8_PATH}"
echo "🔑 Wrote key to ${P8_PATH}"

# ---- 4) Загружаем через altool ----
xcrun altool --upload-app \
  -f "${IPA_PATH}" \
  -t ios \
  --apiKey "${API_KEY_ID}" \
  --apiIssuer "${API_ISSUER_ID}"

echo "✅ Upload IPA to App Store Connect finished successfully"
