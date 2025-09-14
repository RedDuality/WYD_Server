#!/bin/sh
set -e
set -x

# Wait until MinIO is reachable
until mc alias set myminio http://minio-dev:9000 "$MINIO_ROOT_USER" "$MINIO_ROOT_PASSWORD"; do
  echo "MinIO is not ready yet - sleeping..."
  sleep 2
done

# Create buckets
mc mb myminio/events --ignore-existing
mc mb myminio/profiles --ignore-existing

# Create app user
mc admin user add myminio "$MINIO_APP_USER" "$MINIO_APP_PASSWORD"

# Create and attach policy
mc admin policy create myminio app-policy /init-scripts/policy.json
mc admin policy attach myminio app-policy --user "$MINIO_APP_USER"

echo "✅ MinIO initialization complete."