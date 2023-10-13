mkdir -p data_sources && curl -s "http://grafana:3000/api/datasources"  -u admin:admin | jq -c -M '.[]' | split -l 1 - data_sources/

